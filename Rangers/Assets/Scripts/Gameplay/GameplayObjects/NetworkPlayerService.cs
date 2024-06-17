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

        private NetworkPlayer m_OwnerNetworkPlayerView;

        /// <summary>
        /// This is called to configure the NetworkPlayerView for all clients.
        /// It will be called on each clients from the server.
        /// /// </summary>
        [ClientRpc]
        public void ConfigureNetworkPlayerView_ClientRpc()
        {
            List<NetworkPlayer> ActivePlayers = NetworkPlayerViewClientCache.ActivePlayers;

            if (ActivePlayers == null)
            {
                Debug.LogError("ActivePlayers is null");
                return;
            }

            foreach (NetworkPlayer networkPlayer in ActivePlayers)
            {
                if (networkPlayer.IsOwner)
                {
                    m_OwnerNetworkPlayerView = networkPlayer;
                    PlayerInputs playerInputs = new PlayerInputs();
                    playerInputs.Initialize();
                    networkPlayer.SetPlayerInputs(playerInputs);
                    networkPlayer.Init();// this need to be camera target
                }

                if (networkPlayer.IsServer)
                {
                    networkPlayer.SetPlayerModel(new PlayerModel(m_PlayerData));
                    networkPlayer.SetPlayerService(this);
                    CreateEntityForNetworkPlayerView(networkPlayer);
                }
                else
                {
                    networkPlayer.SpawnGraphics();
                }


                // Set Camera Target for owners
                if (networkPlayer.IsOwner)
                {
                    m_PVCamera.SetFollowTarget(networkPlayer.CameraTarget);
                }
            }
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

        private void CreateEntityForNetworkPlayerView(NetworkPlayer networkPlayerView)
        {
            TagSO tag = networkPlayerView.Tag;
            m_PlayerStats.EntityTagSelected.Value = tag;            // Pre set the entity tag to the selected entity tag for later use. later players can change entities by changing the tag

            bool entityFound = TryGetEntity(tag, out IEntityBrain entity);
            if (!entityFound)
                return;

            networkPlayerView.SetEntityBrain(entity);
        }

        private bool TryGetEntity(TagSO tag, out IEntityBrain entity)
        {
            entity = m_EntityFactoryContainer.GetFactory(tag).GetItem();
            return entity != null;
        }
    }
}