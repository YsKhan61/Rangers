using BTG.Player;
using BTG.Utilities;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using VContainer;


namespace BTG.Gameplay.GameplayObjects
{
    public class NetworkPlayerService : NetworkBehaviour, IPlayerService
    {
        [SerializeField]
        [Tooltip("A collection of locations for spawning players")]
        private Transform[] m_PlayerSpawnPoints;

        [SerializeField]
        private PlayerVirtualCamera m_PVCamera;

        [Inject]
        private PlayerStatsSO m_PlayerStats;

        private NetworkAvatarGuidState m_NetworkAvatarGuidState;
        private List<Transform> m_PlayerSpawnPointsList = null;

        private void Awake()
        {
            m_NetworkAvatarGuidState = GetComponent<NetworkAvatarGuidState>();
        }

        public override void OnNetworkSpawn()
        {
            // Create the player controller and input
            // This will replace PlayerService of SinglePlayerGameState
        }

        /// <summary>
        /// This is called on the server when a new player is added to the game.
        /// This consists of the PlayerController , PlayerInput, and PlayerVirtualCamera.
        /// </summary>
        /// <param name="tag"></param>
        [ClientRpc]
        public void CreatePlayer_ClientRpc()
        {
           // Fetch all the NetworkPlayerView from ClientCache
           // Check the Registered Entity Data of the NetworkAvatarGuidState
           // If this is owner, then create player controller, input, and virtual camera
           // For all clients (owner or not) Add the player view as parent of NetworkPlayerView

            Debug.Log("CreatePlayer_ClientRpc" + OwnerClientId);    
        }

        public void OnEntityInitialized(Sprite icon)
        {
            
        }

        public void OnPlayerDeath()
        {
            
        }

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