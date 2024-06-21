using BTG.Actions.UltimateAction;
using BTG.Entity;
using BTG.Player;
using BTG.Utilities;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using VContainer;


namespace BTG.Gameplay.GameplayObjects
{
    public class NetworkPlayerService : NetworkBehaviour
    {
        [SerializeField]
        private PlayerVirtualCamera m_PVCamera;
        public PlayerVirtualCamera PVCamera => m_PVCamera;

        [Inject]
        private PlayerDataSO m_PlayerData;

        [Inject]
        private IObjectResolver m_ObjectResolver;

        [Inject]
        private EntityDataContainerSO m_EntityDataContainer;

        [Inject]
        private UltimateActionDataContainerSO m_UltimateActionDataContainer;

        [Inject]
        private PlayerStatsSO m_PlayerStats;
        public PlayerStatsSO PlayerStats => m_PlayerStats;

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
            persistentPlayer.NetworkEntityGuidState.RegisterEntityData_ClientRpc(selectedEntityGuid);
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

            if (networkPlayer.IsOwner)      // Clients in their own machine
            {
                m_OwnerNetworkPlayer = networkPlayer;
                PlayerInputs playerInputs = new PlayerInputs();
                playerInputs.Initialize();
                networkPlayer.SetPlayerInputs(playerInputs);
                networkPlayer.SetPlayerService(this);
                networkPlayer.SetPlayerModel(new PlayerModel(m_PlayerData));
                networkPlayer.Init();
            }
            if (networkPlayer.IsServer && !networkPlayer.IsOwner)       // Clients in host machine except host
            {
                networkPlayer.SetPlayerModel(new PlayerModel(m_PlayerData));
            }

            ConfigureEntityWithDelay(networkPlayer);
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

        /// This time delay will be sufficient for the network player to subscribe to required events on OnNetworkSpawn
        private async void ConfigureEntityWithDelay(NetworkPlayer networkPlayer)
        {
            await Task.Delay(1000);
            networkPlayer.ConfigureEntity();
        }
    }
}