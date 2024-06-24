using BTG.Utilities;
using UnityEngine;


namespace BTG.Tank
{
    /// <summary>
    /// A pool for the TankBrain
    /// </summary>
    /*public class TankPool : GenericObjectPool<TankBrain>
    {
        [Inject]
        private IObjectResolver m_Resolver;

        private TankDataSO m_TankData;

        public TankPool(TankDataSO data)
        {
            m_TankData = data;
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
            TankModel model = new TankModel(m_TankData);
            TankView view = Object.Instantiate(m_TankData.TankViewPrefab, Container);

            TankBrain brain = new TankBrain.Builder()
                .WithTankModel(model)
                .WithTankPool(this)
                .WithTankView(view)
                .Build();

            m_Resolver.Inject(brain);
            view.SetBrain(brain);
            brain.CreatePrimaryAction();
            brain.CreateUltimateAction();

            return brain;
        }
    }*/

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
        public void ReturnTank(TankView tank)
        {
            ReturnItem(tank);
            tank.gameObject.SetActive(false);
            tank.transform.SetParent(Container);
        }

        protected override TankView CreateItem()
        {
            return Object.Instantiate(m_Prefab, Container);
        }

    }
}