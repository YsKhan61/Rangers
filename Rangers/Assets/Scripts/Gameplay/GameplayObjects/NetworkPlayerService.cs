using BTG.Entity;
using BTG.Gameplay.UI;
using BTG.Player;
using BTG.Utilities;
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

        [SerializeField]
        private NetworkPlayerStatsUI m_NetworkPlayerStatsUI;

        [Inject]
        private PlayerDataSO m_PlayerData;

        [Inject]
        private IObjectResolver m_ObjectResolver;

        [Inject]
        private EntityDataContainerSO m_EntityDataContainer;

        [Inject]
        private PlayerStatsSO m_PlayerStats;
        public PlayerStatsSO PlayerStats => m_PlayerStats;

        private NetworkPlayer m_OwnerNetworkPlayer;
        private CancellationTokenSource m_CTS;

        public override void OnNetworkSpawn()
        {
            m_CTS = new CancellationTokenSource();
            m_PlayerStats.EntityTagSelected.OnValueChanged += OnEntityTagSelectedChanged;

            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }

        public override void OnNetworkDespawn()
        {
            m_PlayerStats.EntityTagSelected.OnValueChanged -= OnEntityTagSelectedChanged;
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_CTS);

            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
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

            m_NetworkPlayerStatsUI.AddPlayerStatRow(clientId);
        }

        public void OnPlayerDeath()
        {
            // Hide the owner UIs (primary, ultimate, icon, health etc)
        }

        public void ShowHeroSelectionUI()
        {
            _ = HelperMethods.InvokeAfterAsync(
                RESPAWN_DELAY,
                () => m_PlayerData.ShowHeroSelectionUI.RaiseEvent(),
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

        private void OnClientDisconnect(ulong clientId)
        {
            m_NetworkPlayerStatsUI.RemovePlayerStatRow(clientId);
        }
    }
}