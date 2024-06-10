using BTG.Utilities;
using UnityEngine;

namespace BTG.Actions.PrimaryAction
{
    public class  TeslaBallPool : GenericObjectPool<TeslaBallView>
    {
        private TeslaFiringDataSO m_Data;
        private Transform m_Container;
        public Transform Container => m_Container;

        public TeslaBallPool(TeslaFiringDataSO data)
        {
            m_Data = data;
            m_Container = new GameObject("TeslaBallContainer").transform;
        }

        public TeslaBallView GetTeslaBall() => GetItem();

        public void ReturnTeslaBall(TeslaBallView teslaBall) => ReturnItem(teslaBall);
        protected override TeslaBallView CreateItem()
        {
            TeslaBallView view = Object.Instantiate(m_Data.TeslaBallViewPrefab, m_Container);
            view.SetPool(this);
            return view;
        }
    }
}

