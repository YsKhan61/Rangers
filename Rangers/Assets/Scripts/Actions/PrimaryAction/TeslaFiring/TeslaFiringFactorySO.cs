using UnityEngine;
using VContainer;

namespace BTG.Actions.PrimaryAction
{

    [CreateAssetMenu(fileName = "Tesla Firing Factory", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/TeslaFiringFactorySO")]
    public class TeslaFiringFactorySO : PrimaryActionFactorySO
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

        NetworkTeslaBallPool m_NetworkPool;
        NetworkTeslaBallPool NetworkPool
        {
            get
            {
                if (m_NetworkPool == null)
                {
                    m_NetworkPool = new NetworkTeslaBallPool(m_Data);
                    m_Resolver.Inject(m_NetworkPool);
                }
                return m_NetworkPool;
            }
        }

        public override IPrimaryAction GetItem()
        {
            TeslaFiring tf = new (m_Data, Pool);
            m_Resolver.Inject(tf);
            return tf;
        }

        public override IPrimaryAction GetNetworkItem()
        {
            TeslaFiring tf = new (m_Data, NetworkPool);
            m_Resolver.Inject(tf);
            return tf;
        }
    }
}

