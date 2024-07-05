using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    public class NetworkAutoTargetView : AutoTargetView, INetworkFiringView
    {
        public ulong ActorOwnerClientId => m_Controller.Actor.OwnerClientId;
    }
}