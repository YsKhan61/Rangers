using BTG.Utilities;
using UnityEngine;

namespace BTG.Tank
{
    public class TankPool : GenericObjectPool<TankBrain>
    {
        private TankDataSO m_TankData;
        private Transform m_TankContainer;
        public Transform TankContainer => m_TankContainer;
        

        public TankPool(TankDataSO data)
        {
            m_TankData = data;
            m_TankContainer = new GameObject("TankContainer of " + data.name).transform;
        }

        public TankBrain GetTank()
        {
            TankBrain tank = GetItem();
            tank.Init();
            return tank;
        }

        public void ReturnTank(TankBrain tank) => ReturnItem(tank);

        protected override TankBrain CreateItem() => new TankBrain(m_TankData, this);
    }
}