using BTG.ConnectionManagement;
using BTG.Gameplay.GameplayObjects;
using BTG.Utilities;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.BossRoom;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;


namespace BTG.Gameplay.GameState
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class MultiplayGameState : GameStateBehaviour
    {
        [SerializeField]
        [Tooltip("Make sure this is included in the NetworkManager's list of prefabs!")]
        private NetworkObject m_PlayerPrefab;

        [SerializeField]
        private NetworkPlayerService m_NetworkPlayerService;

        public override GameState ActiveState { get { return GameState.Multiplay; } }

        /// <summary>
        /// Has the ServerBossRoomState already hit its initial spawn? (i.e. spawned players following load from character select).
        /// </summary>
        public bool InitialSpawnDone { get; private set; }

        private NetcodeHooks m_NetcodeHooks;

        protected override void Awake()
        {
            base.Awake();
            m_NetcodeHooks = GetComponent<NetcodeHooks>();
            m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
            m_NetcodeHooks.OnNetworkDespawnHook += OnNetworkDespawn;
        }

        private void OnNetworkSpawn()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                enabled = false;
                return;
            }

            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
            NetworkManager.Singleton.SceneManager.OnSynchronizeComplete += OnSynchronizeComplete;

            SessionManager<SessionPlayerData>.Instance.OnSessionStarted();
        }

        private void OnNetworkDespawn()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
            NetworkManager.Singleton.SceneManager.OnSynchronizeComplete -= OnSynchronizeComplete;
        }

        protected override void OnDestroy()
        {
            if (m_NetcodeHooks)
            {
                m_NetcodeHooks.OnNetworkSpawnHook -= OnNetworkSpawn;
                m_NetcodeHooks.OnNetworkDespawnHook -= OnNetworkDespawn;
            }

            base.OnDestroy();
        }

        void OnSynchronizeComplete(ulong clientId)
        {
            /*if (InitialSpawnDone && !ServerCharactersCachedInServerMachine.GetServerCharacter(clientId))
            {
                //somebody joined after the initial spawn. This is a Late Join scenario. This player may have issues
                //(either because multiple people are late-joining at once, or because some dynamic entities are
                //getting spawned while joining. But that's not something we can fully address by changes in
                //ServerBossRoomState.
                SpawnPlayer(clientId, true);
            }*/

            Debug.Log("Late joiner: " + clientId + " has completed synchronizing.");
        }

        void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            if (!InitialSpawnDone && loadSceneMode == LoadSceneMode.Single)
            {
                InitialSpawnDone = true;
                foreach (var kvp in NetworkManager.Singleton.ConnectedClients)
                {
                    SpawnNetworkPlayerViewForEachClients(kvp.Key, false);
                }

                m_NetworkPlayerService.CreatePlayer_ClientRpc();
            }
        }

        void OnClientDisconnect(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                // This client (which is a server) disconnects. We should go back to the character select screen.
                SceneLoaderWrapper.Instance.LoadScene("CharSelect", useNetworkSceneManager: true);
            }
        }

        void SpawnNetworkPlayerViewForEachClients(ulong clientId, bool lateJoin)
        {
            // Transform spawnPoint = GetRandomSpawnPoint();

            NetworkObject playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);

            // NetworkObject newPlayer = Instantiate(m_PlayerPrefab, spawnPoint.position, spawnPoint.rotation);
            NetworkObject newPlayer = Instantiate(m_PlayerPrefab, Vector3.zero, Quaternion.identity);

            var persistentPlayerExists = playerNetworkObject.TryGetComponent(out PersistentPlayer persistentPlayer);
            Assert.IsTrue(persistentPlayerExists,
                $"Matching persistent PersistentPlayer for client {clientId} not found!");

            // pass character type from persistent player to avatar
            var networkAvatarGuidStateExists =
                newPlayer.TryGetComponent(out NetworkAvatarGuidState networkAvatarGuidState);                       // NetworkAvatarGuidState

            Assert.IsTrue(networkAvatarGuidStateExists,
                $"NetworkCharacterGuidState not found on player avatar!");

            // if reconnecting, set the player's position and rotation to its previous state
            if (lateJoin)
            {
                SessionPlayerData? sessionPlayerData = SessionManager<SessionPlayerData>.Instance.GetPlayerData(clientId);
                if (sessionPlayerData is { HasCharacterSpawned: true })
                {
                    newPlayer.transform.SetPositionAndRotation(sessionPlayerData.Value.PlayerPosition, sessionPlayerData.Value.PlayerRotation);
                }
            }

            networkAvatarGuidState.n_EntityNetworkGuid.Value =
                persistentPlayer.NetworkAvatarGuidState.n_EntityNetworkGuid.Value;

            // pass name from persistent player to avatar
            if (newPlayer.TryGetComponent(out NetworkNameState networkNameState))                                   // NetworkNameState
            {
                networkNameState.Name.Value = persistentPlayer.NetworkNameState.Name.Value;
            }

            // spawn players characters with destroyWithScene = true
            newPlayer.SpawnWithOwnership(clientId, true);
        }
    }
}
