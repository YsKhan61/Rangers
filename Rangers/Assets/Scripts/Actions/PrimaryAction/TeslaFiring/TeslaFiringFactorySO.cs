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
        TeslaBallPool Pool => m_Pool ??= InitializePool();

        TeslaBallPool m_NetworkPool;
        TeslaBallPool NetworkPool => m_NetworkPool ??= InitializeNetworkPool();
        

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

        TeslaBallPool InitializePool()
        {
            var pool = new TeslaBallPool(m_Data.TeslaBallViewPrefab);
            m_Resolver.Inject(pool);
            return pool;
        }

        TeslaBallPool InitializeNetworkPool()
        {
            var pool = new TeslaBallPool(m_Data.NetworkTeslaBallViewPrefab);
            m_Resolver.Inject(pool);
            return pool;
        }
    }
}

