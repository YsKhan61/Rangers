using BTG.ConnectionManagement;
using BTG.Gameplay.GameplayObjects;
using System;
using System.Collections;
using Unity.Multiplayer.Samples.BossRoom;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;
using VContainer;
using BTG.Utilities;
using BTG.Entity;


namespace BTG.Gameplay.GameState
{
    /// <summary>
    /// Server specialization of Character Select game state.
    /// </summary>
    [RequireComponent(typeof(NetcodeHooks), typeof(NetworkCharSelection))]
    public class ServerCharSelectGameState : GameStateBehaviour
    {
        [SerializeField]
        NetcodeHooks m_NetcodeHooks;

        public override GameState ActiveState => GameState.CharSelect;

        public NetworkCharSelection m_NetworkCharSelection { get; private set; }

        Coroutine m_WaitToEndLobbyCoroutine;

        [Inject]
        ConnectionManager m_ConnectionManager;

        [Inject]
        private SceneNameListSO _sceneNameList;

        protected override void Awake()
        {
            base.Awake();
            m_NetworkCharSelection = GetComponent<NetworkCharSelection>();
            m_NetworkCharSelection.ConnectionManager = m_ConnectionManager;

            m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
            m_NetcodeHooks.OnNetworkDespawnHook += OnNetworkDespawn;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (m_NetcodeHooks)
            {
                m_NetcodeHooks.OnNetworkSpawnHook -= OnNetworkSpawn;
                m_NetcodeHooks.OnNetworkDespawnHook -= OnNetworkDespawn;
            }
        }

        private void OnNetworkSpawn()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                enabled = false;
            }
            else
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
                m_NetworkCharSelection.OnClientChangedSeat += OnClientChangedSeat;

                NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
            }
        }

        private void OnNetworkDespawn()
        {
            if (NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
                NetworkManager.Singleton.SceneManager.OnSceneEvent -= OnSceneEvent;
            }
            if (m_NetworkCharSelection)
            {
                m_NetworkCharSelection.OnClientChangedSeat -= OnClientChangedSeat;
            }
        }

        private void OnClientDisconnectCallback(ulong clientId)
        {
            // clear this client's PlayerNumber and any associated visuals (so other players know they're gone).
            for (int i = 0; i < m_NetworkCharSelection.LobbyPlayers.Count; ++i)
            {
                if (m_NetworkCharSelection.LobbyPlayers[i].ClientId == clientId)
                {
                    m_NetworkCharSelection.LobbyPlayers.RemoveAt(i);
                    break;
                }
            }

            if (!m_NetworkCharSelection.IsLobbyClosed.Value)
            {
                // If the lobby is not already closing, close if the remaining players are all ready
                CloseLobbyIfReady();
            }
        }

        private void OnClientChangedSeat(ulong clientId, int newSeatIdx, bool lockedIn)
        {
            int idx = FindLobbyPlayerIdx(clientId);
            if (idx == -1)
            {
                throw new Exception($"OnClientChangedSeat: client ID {clientId} is not a lobby player and cannot change seats! Shouldn't be here!");
            }

            if (m_NetworkCharSelection.IsLobbyClosed.Value)
            {
                // The user tried to change their class after everything was locked in... too late! Discard this choice
                return;
            }

            if (newSeatIdx == -1)
            {
                // we can't lock in with no seat
                lockedIn = false;
            }
            else
            {
                // see if someone has already locked-in that seat! If so, too late... discard this choice
                foreach (NetworkCharSelection.LobbyPlayerState playerInfo in m_NetworkCharSelection.LobbyPlayers)
                {
                    if (playerInfo.ClientId != clientId && playerInfo.SeatIdx == newSeatIdx && playerInfo.SeatState == NetworkCharSelection.SeatState.LockedIn)
                    {
                        // somebody already locked this choice in. Stop!
                        // Instead of granting lock request, change this player to Inactive state.
                        m_NetworkCharSelection.LobbyPlayers[idx] = new NetworkCharSelection.LobbyPlayerState(
                            clientId,
                            m_NetworkCharSelection.LobbyPlayers[idx].PlayerName,
                            m_NetworkCharSelection.LobbyPlayers[idx].PlayerNumber,
                            NetworkCharSelection.SeatState.Inactive);

                        // then early out
                        return;
                    }
                }
            }

            m_NetworkCharSelection.LobbyPlayers[idx] = new NetworkCharSelection.LobbyPlayerState(
                clientId,
                m_NetworkCharSelection.LobbyPlayers[idx].PlayerName,
                m_NetworkCharSelection.LobbyPlayers[idx].PlayerNumber,
                lockedIn ? NetworkCharSelection.SeatState.LockedIn : NetworkCharSelection.SeatState.Active,
                newSeatIdx,
                Time.time);

            if (lockedIn)
            {
                // to help the clients visually keep track of who's in what seat, we'll "kick out" any other players
                // who were also in that seat. (Those players didn't click "Ready!" fast enough, somebody else took their seat!)
                for (int i = 0; i < m_NetworkCharSelection.LobbyPlayers.Count; ++i)
                {
                    if (m_NetworkCharSelection.LobbyPlayers[i].SeatIdx == newSeatIdx && i != idx)
                    {
                        // change this player to Inactive state.
                        m_NetworkCharSelection.LobbyPlayers[i] = new NetworkCharSelection.LobbyPlayerState(
                            m_NetworkCharSelection.LobbyPlayers[i].ClientId,
                            m_NetworkCharSelection.LobbyPlayers[i].PlayerName,
                            m_NetworkCharSelection.LobbyPlayers[i].PlayerNumber,
                            NetworkCharSelection.SeatState.Inactive);
                    }
                }
            }

            CloseLobbyIfReady();
        }

        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            // We need to filter out the event that are not a client has finished loading the scene
            if (sceneEvent.SceneEventType != SceneEventType.LoadComplete) return;
            // When the client finishes loading the Lobby Map, we will need to Seat it
            SeatNewPlayer(sceneEvent.ClientId);
        }

        /// <summary>
        /// Returns the index of a client in the master LobbyPlayer list, or -1 if not found
        /// </summary>
        private int FindLobbyPlayerIdx(ulong clientId)
        {
            for (int i = 0; i < m_NetworkCharSelection.LobbyPlayers.Count; ++i)
            {
                if (m_NetworkCharSelection.LobbyPlayers[i].ClientId == clientId)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Looks through all our connections and sees if everyone has locked in their choice;
        /// if so, we lock in the whole lobby, save state, and begin the transition to gameplay
        /// </summary>
        private void CloseLobbyIfReady()
        {
            foreach (NetworkCharSelection.LobbyPlayerState playerInfo in m_NetworkCharSelection.LobbyPlayers)
            {
                if (playerInfo.SeatState != NetworkCharSelection.SeatState.LockedIn)
                    return; // nope, at least one player isn't locked in yet!
            }

            // everybody's ready at the same time! Lock it down!
            m_NetworkCharSelection.IsLobbyClosed.Value = true;

            // remember our choices so the next scene can use the info
            SaveLobbyResults();

            // Delay a few seconds to give the UI time to react, then switch scenes
            m_WaitToEndLobbyCoroutine = StartCoroutine(WaitToEndLobby());
        }

        void SaveLobbyResults()
        {
            foreach (NetworkCharSelection.LobbyPlayerState playerInfo in m_NetworkCharSelection.LobbyPlayers)
            {
                var playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(playerInfo.ClientId);

                if (playerNetworkObject && playerNetworkObject.TryGetComponent(out PersistentPlayer persistentPlayer))
                {
                    // pass avatar GUID to PersistentPlayer
                    // it'd be great to simplify this with something like a NetworkScriptableObjects :(

                    EntityDataSO entityData = m_NetworkCharSelection.EntityDataContainer.GetEntityDataBySeatIndex(playerInfo.SeatIdx);

                    /*persistentPlayer.NetworkEntityGuidState.n_NetworkEntityGuid.Value =
                        entityData.Guid.ToNetworkGuid();*/
                    persistentPlayer.NetworkEntityGuidState.RegisterEntityData_ClientRpc(entityData.Guid.ToNetworkGuid());
                }
            }
        }

        private void SeatNewPlayer(ulong clientId)
        {
            // If lobby is closing and waiting to start the game, cancel to allow that new player to select a character
            if (m_NetworkCharSelection.IsLobbyClosed.Value)
            {
                CancelCloseLobby();
            }

            SessionPlayerData? sessionPlayerData = SessionManager<SessionPlayerData>.Instance.GetPlayerData(clientId);
            if (sessionPlayerData.HasValue)
            {
                var playerData = sessionPlayerData.Value;
                if (playerData.PlayerNumber == -1 || !IsPlayerNumberAvailable(playerData.PlayerNumber))
                {
                    // If no player num already assigned or if player num is no longer available, get an available one.
                    playerData.PlayerNumber = GetAvailablePlayerNumber();
                }
                if (playerData.PlayerNumber == -1)
                {
                    // Sanity check. We ran out of seats... there was no room!
                    throw new Exception($"we shouldn't be here, connection approval should have refused this connection already for client ID {clientId} and player num {playerData.PlayerNumber}");
                }

                m_NetworkCharSelection.LobbyPlayers.Add(new NetworkCharSelection.LobbyPlayerState(
                    clientId,
                    playerData.PlayerName,
                    playerData.PlayerNumber,
                    NetworkCharSelection.SeatState.Inactive));
                SessionManager<SessionPlayerData>.Instance.SetPlayerData(clientId, playerData);
            }
        }

        /// <summary>
        /// Cancels the process of closing the lobby, so that if a new player joins, they are able to chose a character.
        /// </summary>
        private void CancelCloseLobby()
        {
            if (m_WaitToEndLobbyCoroutine != null)
            {
                StopCoroutine(m_WaitToEndLobbyCoroutine);
            }
            m_NetworkCharSelection.IsLobbyClosed.Value = false;
        }

        private bool IsPlayerNumberAvailable(int playerNumber)
        {
            bool found = false;
            foreach (NetworkCharSelection.LobbyPlayerState playerState in m_NetworkCharSelection.LobbyPlayers)
            {
                if (playerState.PlayerNumber == playerNumber)
                {
                    found = true;
                    break;
                }
            }

            return !found;
        }

        private int GetAvailablePlayerNumber()
        {
            for (int possiblePlayerNumber = 0; possiblePlayerNumber < m_ConnectionManager.MaxConnectedPlayers; ++possiblePlayerNumber)
            {
                if (IsPlayerNumberAvailable(possiblePlayerNumber))
                {
                    return possiblePlayerNumber;
                }
            }
            // we couldn't get a Player# for this person... which means the lobby is full!
            return -1;
        }

        IEnumerator WaitToEndLobby()
        {
            /// Once a client chooses a seat, server need little time to register the network entity data from network guid in NetworkEntityGuidState.
            /// This involves a delay to update the network variable of NetworkEntityGuidState and then register the entity data.
            /// Once it's done, then we can save the lobby seat selection and start the game.
            yield return new WaitForSeconds(5); 
            SceneLoaderWrapper.Instance.LoadScene(_sceneNameList.MultiplayerScene, useNetworkSceneManager: true);
        }
    }
}
