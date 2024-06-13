using BTG.Factory;
using UnityEngine;
using VContainer;

namespace BTG.Actions.PrimaryAction
{

    [CreateAssetMenu(fileName = "Tesla Firing Factory", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/TeslaFiringFactorySO")]
    public class TeslaFiringFactorySO : FactorySO<IPrimaryAction>
    {
        [Inject]
        private IObjectResolver m_Resolver;

        [SerializeField]
        TeslaFiringDataSO m_Data;

        TeslaBallPool m_Pool;
        TeslaBallPool Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    m_Pool = new TeslaBallPool(m_Data);
                    m_Resolver.Inject(m_Pool);
                }
                return m_Pool;
            }
        }

        public override IPrimaryAction GetItem()
        {
            TeslaFiring tf = new (m_Data, Pool);
            m_Resolver.Inject(tf);
            return tf;
        }
    }
}

