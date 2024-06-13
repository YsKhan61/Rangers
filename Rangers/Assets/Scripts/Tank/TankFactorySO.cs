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
                    m_Pool = new(m_Data);
                    m_Resolver.Inject(m_Pool);
                }
                return m_Pool;
            }
        }

        public override IEntityBrain GetItem()
        {
            TankBrain brain = Pool.GetTank();
            m_Resolver.Inject(brain);
            return brain;
        }
    }
}


