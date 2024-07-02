using BTG.Actions.UltimateAction;
using BTG.Entity;
using BTG.Events;
using BTG.Player;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using VContainer;


namespace BTG.Gameplay.GameplayObjects
{
    public class NetworkPlayerService : NetworkBehaviour
    {
        private const int RESPAWN_DELAY = 2;

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
        private CancellationTokenSource m_CTS;

        public override void OnNetworkSpawn()
        {
            m_CTS = new CancellationTokenSource();
            m_PlayerStats.ResetStats();
            m_PlayerStats.EntityTagSelected.OnValueChanged += OnEntityTagSelectedChanged;
        }

        public override void OnNetworkDespawn()
        {
            m_PlayerStats.EntityTagSelected.OnValueChanged -= OnEntityTagSelectedChanged;
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_CTS);
        }

        [ServerRpc(RequireOwnership = false)]
        public void OnEntityTagSelectionChanged_ServerRpc(ulong playerOwnerClientId, NetworkGuid selectedEntityGuid)
        {
            // Debug.Log($"PlayerOwnerClient {playerOwnerClientId} Calling OnEntityTagSelectionChanged_ServerRpc!");
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
            networkPlayer.SetPlayerModel(new PlayerModel(m_PlayerData));

            if (networkPlayer.IsOwner)      // Clients in their own machine
            {
                m_OwnerNetworkPlayer = networkPlayer;
                PlayerInputs playerInputs = new PlayerInputs();
                playerInputs.Initialize();
                networkPlayer.SetPlayerInputs(playerInputs);
                networkPlayer.SetPlayerService(this);
            }

            ConfigureEntityWithDelay(networkPlayer);
        }

        public void OnPlayerDeath()
        {
            if (!IsServer) return;

            m_PlayerStats.DeathCount.Value++;
        }

        public void ShowHeroSelectionUI()
        {
            _ = HelperMethods.InvokeAfterAsync(
                RESPAWN_DELAY,
                () => EventBus<ShowEntitySelectUIEventData>.Invoke(new ShowEntitySelectUIEventData { }),
                m_CTS.Token);
        }

        private void OnEntityTagSelectedChanged()
        {
            EntityDataSO entityData = m_EntityDataContainer.GetEntityData(m_PlayerStats.EntityTagSelected.Value);
            OnEntityTagSelectionChanged_ServerRpc(m_OwnerNetworkPlayer.OwnerClientId, entityData.Guid.ToNetworkGuid());
        }

        private async void ConfigureEntityWithDelay(NetworkPlayer networkPlayer)
        {
            // NOTE - This time delay will be sufficient for the network player to subscribe to required events on OnNetworkSpawn
            await Task.Delay(1000);
            networkPlayer.Configure();
        }
    }
}