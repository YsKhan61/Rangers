using BTG.Utilities;
using Unity.Netcode;


namespace BTG.Actions.UltimateAction
{
    public class NetworkAutoTargetView : AutoTargetView, INetworkFiringView
    {
        public ulong ActorOwnerClientId => m_Controller.Actor.OwnerClientId;

        protected override void Despawn()
        {
            isLaunched = false;
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}