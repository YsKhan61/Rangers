using BTG.Utilities;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;
using VContainer;


namespace BTG.Gameplay
{
    /// <summary>
    /// Send and receive chat messages on the server.
    /// </summary>
    public class ServerChatSystem : MonoBehaviour
    {

        [SerializeField]
        private NetcodeHooks m_netcodeHooks;

        private void Awake()
        {
            m_netcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
        }

        private void OnDestroy()
        {
            m_netcodeHooks.OnNetworkSpawnHook -= OnNetworkSpawn;
        }

        private void OnNetworkSpawn()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                enabled = false;
                return;
            }
        }

        [Inject]
        IPublisher<NetworkChatMessage> m_networkClientChatPublisher;

        [ServerRpc(RequireOwnership = false)]
        public void SendChatMessageServerRpc(NetworkChatMessage message)
        {
            m_networkClientChatPublisher.Publish(message);
        }
    }
}

