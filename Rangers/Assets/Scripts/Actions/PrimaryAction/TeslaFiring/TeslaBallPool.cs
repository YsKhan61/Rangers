using BTG.Utilities;
using UnityEngine;
using VContainer;

namespace BTG.Actions.PrimaryAction
{
    public class  TeslaBallPool : GenericObjectPool<TeslaBallView>
    {
        private TeslaFiringDataSO m_Data;

        [Inject]
        private IObjectResolver m_Resolver;

        public TeslaBallPool(TeslaFiringDataSO data)
        {
            m_Data = data;
        }

        public TeslaBallView GetTeslaBall() => GetItem();

        public void ReturnTeslaBall(TeslaBallView teslaBall) => ReturnItem(teslaBall);
        protected override TeslaBallView CreateItem()
        {
            TeslaBallView view = Object.Instantiate(m_Data.TeslaBallViewPrefab, Container);
            view.SetPool(this);
            m_Resolver.Inject(view);
            return view;
        }
    }
}

