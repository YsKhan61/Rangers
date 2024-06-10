using BTG.Utilities;
using Unity.Netcode;
using VContainer;


namespace BTG.Gameplay
{
    /// <summary>
    /// Send and receive chat messages on the server.
    /// This is a server-only component.
    /// </summary>
    public class ServerChatSystem : NetworkBehaviour
    {
        [Inject]
        IPublisher<NetworkChatMessage> m_networkClientChatPublisher;

        public override void OnNetworkSpawn()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                Destroy(this);
            }

            DontDestroyOnLoad(gameObject);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendChatMessageServerRpc(NetworkChatMessage message)
        {
            m_networkClientChatPublisher.Publish(message);
        }
    }
}