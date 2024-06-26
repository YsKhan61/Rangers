using BTG.Entity;
using UnityEngine;
using VContainer;


namespace BTG.Tank
{
    /// <summary>
    /// This factory is responsible for creating the tanks.
    /// It can create all types of tanks according to the data given
    /// </summary>
    [CreateAssetMenu(fileName = "TankFactory", menuName = "ScriptableObjects/Factory/EntityFactory/TankFactorySO")]
    public class TankFactorySO : EntityFactorySO
    {
        [Inject]
        private IObjectResolver m_Resolver;

        [SerializeField]
        TankDataSO m_Data;

        TankPool m_Pool;
        public TankPool Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    m_Pool = new(m_Data.TankViewPrefab);
                    m_Resolver.Inject(m_Pool);
                }
                return m_Pool;
            }
        }

        public override IEntityBrain GetItem()
        {
            TankModel model = new TankModel(m_Data);
            TankView view = Pool.GetTankView();

            TankBrain brain = new TankBrain.Builder()
                .WithTankModel(model)
                .WithTankPool(Pool)
                .WithTankView(view)
                .Build();

            m_Resolver.Inject(brain);
            view.SetBrain(brain);
            brain.CreatePrimaryAction();
            brain.CreateUltimateAction();
            return brain;
        }

        public override IEntityBrain GetServerItem()
        {
            TankModel model = new TankModel(m_Data);
            TankView view = Pool.GetTankView();

            TankBrain brain = new TankBrain.Builder()
                .WithTankModel(model)
                .WithTankPool(Pool)
                .WithTankView(view)
                .Build();

            m_Resolver.Inject(brain);
            view.SetBrain(brain);
            brain.CreateNetworkPrimaryAction();
            brain.CreateNetworkUltimateAction();
            return brain;
        }

        /// <summary>
        /// This method is used to create the tank for the client side
        /// NOTE - Later we can use a pool for this
        /// </summary>
        /// <returns></returns>
        public override IEntityBrain GetNonServerItem()
        {
            TankModel model = new TankModel(m_Data);
            TankView view = Pool.GetTankView();

            TankBrain brain = new TankBrain.Builder()
                .WithTankModel(model)
                .WithTankPool(Pool)
                .WithTankView(view)
                .Build();

            m_Resolver.Inject(brain);

            // create primary action
            // brain.CreateUltimateAction();
            return brain;
        }
    }
}


