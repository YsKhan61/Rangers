using Unity.Netcode;


namespace BTG.Player
{
    public class NetworkPlayerView : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsLocalPlayer)
            {
                
            }
        }
    }
}