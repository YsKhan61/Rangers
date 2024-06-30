using BTG.Utilities;
using UnityEngine;


namespace BTG.Tank
{
    /// <summary>
    /// A pool for the TankBrain
    /// </summary>

    public class TankPool : GenericObjectPool<TankView>
    {
        private TankView m_Prefab;

        public TankPool(TankView prefab) => m_Prefab = prefab;

        /// <summary>
        /// Get a tank from the pool
        /// </summary>
        public TankView GetTankView() => GetItem();

        /// <summary>
        /// Returns the tank to the pool
        /// </summary>
        public void ReturnTank(TankView tank) => ReturnItem(tank);

        protected override TankView CreateItem()
        {
            return Object.Instantiate(m_Prefab);
        }

    }
}