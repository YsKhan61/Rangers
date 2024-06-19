using BTG.ConnectionManagement;
using BTG.Entity;
using BTG.Gameplay.GameplayObjects;
using BTG.Utilities;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.BossRoom;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using VContainer;


namespace BTG.Gameplay.GameState
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class ServerMultiplayGameState : GameStateBehaviour
    {
        [SerializeField]
        [Tooltip("Make sure this is included in the NetworkManager's list of prefabs!")]
        private NetworkObject m_PlayerPrefab;

        [SerializeField]
        private NetworkPlayerService m_NetworkPlayerService;

        [SerializeField]
        [Tooltip("A collection of locations for spawning players")]
        private Transform[] m_PlayerSpawnPoints;

        public override GameState ActiveState { get { return GameState.Multiplay; } }

        /// <summary>
        /// Has the ServerBossRoomState already hit its initial spawn? (i.e. spawned players following load from character select).
        /// </summary>
        public bool InitialSpawnDone { get; private set; }

        private NetcodeHooks m_NetcodeHooks;
        private List<Transform> m_PlayerSpawnPointsList = null;

        [Inject]
        private IObjectResolver m_ObjectResolver;


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
            if (InitialSpawnDone && !NetworkPlayerClientCache.GetPlayer(clientId))
            {
                //somebody joined after the initial spawn. This is a Late Join scenario. This player may have issues
                //(either because multiple people are late-joining at once, or because some dynamic entities are
                //getting spawned while joining. But that's not something we can fully address by changes in
                //ServerMultiplayGameState.
                SpawnNetworkPlayerForEachClients(clientId, true);

                Debug.Log("Now need to reconfig the network player view");
                // After spawning network player we need to inform NetworkPlayerService to reconfig the network player view
            }
        }

        void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            if (!InitialSpawnDone && loadSceneMode == LoadSceneMode.Single)
            {
                InitialSpawnDone = true;
                foreach (var kvp in NetworkManager.Singleton.ConnectedClients)
                {
                    SpawnNetworkPlayerForEachClients(kvp.Key, false);
                }
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

        void SpawnNetworkPlayerForEachClients(ulong clientId, bool lateJoin)
        {
            NetworkObject playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);

            NetworkObject newPlayer = Instantiate(m_PlayerPrefab);

            var persistentPlayerExists = playerNetworkObject.TryGetComponent(out PersistentPlayer persistentPlayer);
            Assert.IsTrue(persistentPlayerExists,
                $"Matching persistent PersistentPlayer for client {clientId} not found!");

            // if reconnecting, set the player's position and rotation to its previous state
            if (lateJoin)
            {
                SessionPlayerData? sessionPlayerData = SessionManager<SessionPlayerData>.Instance.GetPlayerData(clientId);
                if (sessionPlayerData is { HasCharacterSpawned: true })
                {
                    newPlayer.transform.SetPositionAndRotation(sessionPlayerData.Value.PlayerPosition, sessionPlayerData.Value.PlayerRotation);
                }
            }
            else // else spawn the player at a random spawn point
            {
                Transform spawnPoint = GetRandomSpawnPoint();
                newPlayer.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            }

            newPlayer.SpawnWithOwnership(clientId, true);

            // pass name from persistent player to avatar
            if (newPlayer.TryGetComponent(out NetworkNameState networkNameState))                                   // NetworkNameState
            {
                networkNameState.Name.Value = persistentPlayer.NetworkNameState.Name.Value;
            }

            m_NetworkPlayerService.ConfigureNetworkPlayer_ClientRpc(clientId);
        }

        /// <summary>
        /// This need to be called on the server to set separate position for each client.
        /// </summary>
        /// <returns></returns>
        private Transform GetRandomSpawnPoint()
        {
            Transform spawnPoint;

            if (m_PlayerSpawnPointsList == null || m_PlayerSpawnPointsList.Count == 0)
            {
                m_PlayerSpawnPointsList = new List<Transform>(m_PlayerSpawnPoints);
            }

            Debug.Assert(m_PlayerSpawnPointsList.Count > 0,
                $"PlayerSpawnPoints array should have at least 1 spawn points.");

            int index = UnityEngine.Random.Range(0, m_PlayerSpawnPointsList.Count);
            spawnPoint = m_PlayerSpawnPointsList[index];
            m_PlayerSpawnPointsList.RemoveAt(index);

            return spawnPoint;
        }
    }
}
