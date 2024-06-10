using BTG.Utilities;
using UnityEngine;

namespace BTG.Tank
{
    /// <summary>
    /// A pool for the TankBrain
    /// </summary>
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

        /// <summary>
        /// Get a tank from the pool
        /// </summary>
        /// <returns></returns>
        public TankBrain GetTank() => GetItem();

        /// <summary>
        /// Returns the tank to the pool
        /// </summary>
        /// <param name="tank"></param>
        public void ReturnTank(TankBrain tank) => ReturnItem(tank);

        protected override TankBrain CreateItem()
        {
            TankBrain tankBrain = new (m_TankData, this);
            return tankBrain;
        }
    }
}