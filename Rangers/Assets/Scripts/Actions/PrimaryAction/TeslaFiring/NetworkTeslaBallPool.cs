using BTG.Utilities;
using UnityEngine;
using VContainer;


namespace BTG.Actions.PrimaryAction
{
    public class NetworkTeslaBallPool : GenericObjectPool<NetworkTeslaBallView>
    {
        private NetworkTeslaBallView m_Prefab;

        [Inject]
        protected IObjectResolver m_Resolver;

        public NetworkTeslaBallPool(NetworkTeslaBallView prefab) => m_Prefab = prefab;

        public NetworkTeslaBallView GetTeslaBall() => GetItem();

        public void ReturnTeslaBall(NetworkTeslaBallView item) => ReturnItem(item);

        protected override NetworkTeslaBallView CreateItem()
        {
            NetworkTeslaBallView view = Object.Instantiate(m_Prefab);
            view.SetPool(this);
            m_Resolver.Inject(view);
            view.NetworkObject.Spawn(true);
            return view;
        }
    }
}

