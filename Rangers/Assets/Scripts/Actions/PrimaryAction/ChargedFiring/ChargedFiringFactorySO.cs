using UnityEngine;
using VContainer;


namespace BTG.Actions.PrimaryAction
{
    [CreateAssetMenu(fileName = "Charged Firing Factory", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/ChargedFiringFactorySO")]
    public class ChargedFiringFactorySO : PrimaryActionFactorySO
    {
        [Inject]
        private IObjectResolver m_Resolver;

        [SerializeField]
        ChargedFiringDataSO m_Data;

        ProjectilePool m_Pool;
        ProjectilePool Pool => m_Pool ??= InitializePool();

        NetworkProjectilePool m_NetworkPool;
        NetworkProjectilePool NetworkPool => m_NetworkPool ??= InitializeNetworkPool();

        public override IPrimaryAction GetItem()
        {
            ChargedFiring cf = new (m_Data, Pool);
            m_Resolver.Inject(cf);
            return cf;
        }

        public override IPrimaryAction GetNetworkItem()
        {
            NetworkChargedFiring ncf = new (m_Data, NetworkPool);
            m_Resolver.Inject(ncf);
            return ncf;
        }

        ProjectilePool InitializePool()
        {
            var pool = new ProjectilePool(m_Data.ViewPrefab);
            m_Resolver.Inject(pool);
            return pool;
        }

        NetworkProjectilePool InitializeNetworkPool()
        {
            var pool = new NetworkProjectilePool(m_Data.NetworkViewPrefab);
            m_Resolver.Inject(pool);
            return pool;
        }
    }
}