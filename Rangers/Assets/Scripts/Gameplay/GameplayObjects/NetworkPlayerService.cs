using BTG.Entity;
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
        private IObjectResolver m_ObjectResolver;

        [Inject]
        private EntityDataContainerSO m_EntityDataContainer;

        [Inject]
        private PlayerStatsSO m_PlayerStats;

        private NetworkPlayer m_OwnerNetworkPlayer;

        public override void OnNetworkSpawn()
        {
            m_PlayerStats.EntityTagSelected.OnValueChanged += OnEntityTagSelectedChanged;
        }

        public override void OnNetworkDespawn()
        {
            m_PlayerStats.EntityTagSelected.OnValueChanged -= OnEntityTagSelectedChanged;
        }

        [ServerRpc(RequireOwnership = false)]
        public void OnEntityTagSelectionChanged_ServerRpc(ulong playerOwnerClientId, NetworkGuid selectedEntityGuid)
        {
            Debug.Log($"PlayerOwnerClient {playerOwnerClientId} Calling OnEntityTagSelectionChanged_ServerRpc!");
            var persistentPlayer = PersistentPlayerClientCache.GetPlayer(playerOwnerClientId);
            persistentPlayer.NetworkEntityGuidState.n_NetworkEntityGuid.Value = selectedEntityGuid;
        }

        /// <summary>
        /// This is called to configure the NetworkPlayerView for all clients.
        /// It will be called on each clients from the server.
        /// /// </summary>
        [ClientRpc]
        public void ConfigureNetworkPlayer_ClientRpc(ulong clientId)
        {
            NetworkPlayer networkPlayer = NetworkPlayerClientCache.GetPlayer(clientId);
            if (networkPlayer == null)
            {
                Debug.LogError("Network player is null. This should not happen!");
                return;
            }

            m_ObjectResolver.Inject(networkPlayer);

            if (networkPlayer.IsOwner)
            {
                m_OwnerNetworkPlayer = networkPlayer;
                m_OwnerNetworkPlayer.PVC_Camera = m_PVCamera;
                PlayerInputs playerInputs = new PlayerInputs();
                playerInputs.Initialize();
                networkPlayer.SetPlayerInputs(playerInputs);
                networkPlayer.Init();// this need to be camera target
            }

            if (networkPlayer.IsServer)
            {
                networkPlayer.SetPlayerModel(new PlayerModel(m_PlayerData));
                networkPlayer.SetPlayerService(this);
            }

            networkPlayer.ConfigureEntity();
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

        private void OnEntityTagSelectedChanged()
        {
            EntityDataSO entityData = m_EntityDataContainer.GetEntityData(m_PlayerStats.EntityTagSelected.Value);
            OnEntityTagSelectionChanged_ServerRpc(m_OwnerNetworkPlayer.OwnerClientId, entityData.Guid.ToNetworkGuid());
        }
    }
}