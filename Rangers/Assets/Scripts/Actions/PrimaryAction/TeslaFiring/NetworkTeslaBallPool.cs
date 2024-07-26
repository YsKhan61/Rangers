using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    public class NetworkTeslaBallPool : GenericObjectPool<NetworkTeslaBallView>
    {
        private NetworkTeslaBallView m_Prefab;

        public NetworkTeslaBallPool(NetworkTeslaBallView prefab) => m_Prefab = prefab;

        public NetworkTeslaBallView GetTeslaBall() => GetItem();

        public void ReturnTeslaBall(NetworkTeslaBallView item) => ReturnItem(item);

        protected override NetworkTeslaBallView CreateItem()
        {
            NetworkTeslaBallView view = Object.Instantiate(m_Prefab);
            view.SetPool(this);
            view.NetworkObject.Spawn(true);
            return view;
        }
    }
}

