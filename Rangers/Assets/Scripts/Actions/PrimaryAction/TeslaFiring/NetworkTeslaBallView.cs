using Unity.Netcode;
using UnityEngine;

namespace BTG.Actions.PrimaryAction
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkTeslaBallView : TeslaBallView 
    {
        [SerializeField]
        private NetworkObject m_NetworkObject;

        private void Awake()
        {
            m_NetworkObject = GetComponent<NetworkObject>();
        }
    }

}

