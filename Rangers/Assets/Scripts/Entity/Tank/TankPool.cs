using BTG.Utilities;
using UnityEngine;


namespace BTG.Entity.Tank
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
        public TankView GetTankView()
        {
            TankView view = GetItem();
            view.gameObject.SetActive(true);
            return view;
        }
        /// <summary>
        /// Returns the tank to the pool
        /// </summary>
        public void ReturnTank(TankView tank)
        {
            tank.gameObject.SetActive(false);
            ReturnItem(tank);
        }

        protected override TankView CreateItem() => Object.Instantiate(m_Prefab);

    }
}