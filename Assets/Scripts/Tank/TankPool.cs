using BTG.Utilities;
using UnityEngine;

namespace BTG.Tank
{
    public class TankPool : GenericObjectPool<TankMainController>
    {
        private TankDataSO m_TankData;
        private Transform m_TankContainer;
        public Transform TankContainer => m_TankContainer;
        

        public TankPool(TankDataSO data)
        {
            m_TankData = data;
            m_TankContainer = new GameObject("TankContainer of " + data.name).transform;
        }

        public TankMainController GetTank()
        {
            TankMainController tank = GetItem();
            tank.Init();
            return tank;
        }

        public void ReturnTank(TankMainController tank)
        {
            ReturnItem(tank);
        }

        protected override TankMainController CreateItem() => new TankMainController(m_TankData, this);
    }
}