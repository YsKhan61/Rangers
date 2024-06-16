﻿using BTG.Entity;
using BTG.Player;
using BTG.Utilities;
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

        private PlayerTankController m_Controller;

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
                    m_Controller.SetPose(new Pose(networkPlayerView.transform.position, networkPlayerView.transform.rotation));
                    networkPlayerView.SetFollowTarget(m_Controller.Transform);
                    TagSO entityTagSelected = networkPlayerView.Tag;
                    m_PlayerStats.EntityTagSelected.Value = entityTagSelected;
                    SpawnEntity(entityTagSelected);
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

        private void SpawnEntity(TagSO tag)
        {
            bool entityFound = TryGetEntity(tag, out IEntityBrain entity);
            if (!entityFound)
                return;

            m_Controller.SetEntityBrain(entity);
            m_Controller.Init();
            m_PVCamera.SetFollowTarget(m_Controller.CameraTarget);
        }

        private bool TryGetEntity(TagSO tag, out IEntityBrain entity)
        {
            entity = m_EntityFactoryContainer.GetFactory(tag).GetItem();
            return entity != null;
        }
    }
}