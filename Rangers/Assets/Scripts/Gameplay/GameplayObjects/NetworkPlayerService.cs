using BTG.Entity;
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
        private PlayerVirtualCamera m_PVCamera;

        [Inject]
        private PlayerDataSO m_PlayerData;

        [Inject]
        private EntityFactoryContainerSO m_EntityFactoryContainer;

        [Inject]
        private PlayerStatsSO m_PlayerStats;


        private NetworkAvatarGuidState m_NetworkAvatarGuidState;
        private PlayerTankController m_Controller;

        private void Awake()
        {
            m_NetworkAvatarGuidState = GetComponent<NetworkAvatarGuidState>();
        }

        public override void OnNetworkSpawn()
        {
            CreatePlayerController();
            CreatePlayerInput();
            m_PVCamera.SetFollowTarget(m_Controller.Transform);
        }

        /// <summary>
        /// This is called to configure the NetworkPlayerView for all clients.
        /// It will be called on each clients from the server.
        /// /// </summary>
        [ClientRpc]
        public void ConfigureNetworkPlayerView_ClientRpc()
        {
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
            {
                NetworkPlayerView networkPlayerView = NetworkPlayerViewClientCache.GetPlayerView(clientId);

                if (networkPlayerView == null)
                {
                    Debug.LogError("Player not found in ClientCache. This should not happen.");
                    return;
                }

                if (clientId == OwnerClientId)
                {
                    networkPlayerView.transform.SetParent(m_Controller.Transform);
                    SpawnEntity();
                    SetRandomSpawnPoint_ServerRpc();
                }
                else
                {
                    networkPlayerView.SpawnGraphics();
                }
            }

            Debug.Log("CreatePlayer_ClientRpc" + OwnerClientId);    
        }

        public void OnEntityInitialized(Sprite icon)
        {
            m_PlayerStats.PlayerIcon.Value = icon;
        }

        public void OnPlayerDeath()
        {
            m_PlayerStats.DeathCount.Value++;

            // Respawn
        }

        private void CreatePlayerController()
        {
            m_Controller = new PlayerTankController.Builder()
                .CreateModel(m_PlayerData)
                .CreateView(m_PlayerData.Prefab)
                .WithPlayerService(this)
                .Build();
        }

        private void CreatePlayerInput()
        {
            PlayerInputs playerInput = new(m_Controller);
            playerInput.Initialize();
        }

        private void SpawnEntity()
        {
            bool entityFound = TryGetEntity(out IEntityBrain entity);
            if (!entityFound)
                return;

            m_Controller.SetEntityBrain(entity);
            m_Controller.Init();
            m_PVCamera.SetFollowTarget(m_Controller.CameraTarget);
        }

        private bool TryGetEntity(out IEntityBrain entity)
        {
            entity = m_EntityFactoryContainer.GetFactory(m_PlayerStats.EntityTagSelected.Value).GetItem();
            return entity != null;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetRandomSpawnPoint_ServerRpc()
        {
            m_Controller.SetPose(new Pose(GetRandomSpawnPoint().position, Quaternion.identity));
        }

        
    }
}