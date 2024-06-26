using BTG.Utilities;
using UnityEngine;
using VContainer;


namespace BTG.Actions.PrimaryAction
{
    public class NetworkTeslaBallPool : MonoBehaviourObjectPool<NetworkTeslaBallView>
    {
        private NetworkTeslaBallView m_Prefab;

        [Inject]
        protected IObjectResolver m_Resolver;

        public NetworkTeslaBallPool(NetworkTeslaBallView prefab) => m_Prefab = prefab;

        public NetworkTeslaBallView GetTeslaBall()
        {
            NetworkTeslaBallView item = base.GetItem();
            item.NetworkObject.Spawn(true);
            return item;
        }

        public void ReturnTeslaBall(NetworkTeslaBallView item)
        {
            item.NetworkObject.Despawn(true);
            base.ReturnItem(item);
        }

        protected override NetworkTeslaBallView CreateItem()
        {
            NetworkTeslaBallView view = Object.Instantiate(m_Prefab, Container);
            view.SetPool(this);
            m_Resolver.Inject(view);
            return view;
        }
    }
}

