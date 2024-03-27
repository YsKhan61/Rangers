using BTG.Utilities;
using UnityEngine;

namespace BTG.Tank
{
    public class TankPool : GenericObjectPool<TankController>
    {
        private TankDataSO m_TankData;
        private Transform m_TankContainer;
        public Transform TankContainer => m_TankContainer;
        

        public TankPool(TankDataSO data)
        {
            m_TankData = data;
            m_TankContainer = new GameObject("TankContainer of " + data.name).transform;
        }

        public TankController GetTank()
        {
            TankController tank = GetItem();
            tank.Init();
            return tank;
        }

        public void ReturnTank(TankController tank)
        {
            ReturnItem(tank);
        }

        protected override TankController CreateItem() => new TankController(m_TankData, this);
    }
}