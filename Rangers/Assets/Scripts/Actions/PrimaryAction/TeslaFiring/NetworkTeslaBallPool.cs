using Unity.Netcode;
using UnityEngine;

namespace BTG.Actions.PrimaryAction
{
    public class NetworkTeslaBallPool : TeslaBallPool
    {
        public NetworkTeslaBallPool(TeslaFiringDataSO data) : base(data) { }

        public override TeslaBallView GetTeslaBall()
        {
            TeslaBallView item = base.GetTeslaBall();
            item.GetComponent<NetworkObject>().Spawn();
            return item;
        }

        public override void ReturnTeslaBall(TeslaBallView item)
        {
            item.GetComponent<NetworkObject>().Despawn(true);
            base.ReturnTeslaBall(item);
        }

        protected override TeslaBallView CreateItem()
        {
            TeslaBallView view = Object.Instantiate(m_Data.NetworkTeslaBallViewPrefab, Container);
            view.SetPool(this);
            m_Resolver.Inject(view);
            return view;
        }
    }
}

