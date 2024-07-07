using UnityEngine;
using UnityEngine.SceneManagement;


namespace BTG.Actions.PrimaryAction
{
    [CreateAssetMenu(fileName = "Charged Firing Factory", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/ChargedFiringFactorySO")]
    public class ChargedFiringFactorySO : PrimaryActionFactorySO
    {
        [SerializeField]
        ChargedFiringDataSO m_Data;

        ProjectilePool m_Pool;
        ProjectilePool Pool => m_Pool ??= InitializePool();

        NetworkProjectilePool m_NetworkPool;
        NetworkProjectilePool NetworkPool => m_NetworkPool ??= InitializeNetworkPool();

        public override IPrimaryAction GetItem() => new ChargedFiring(m_Data, Pool);

        public override IPrimaryAction GetNetworkItem() => new NetworkChargedFiring(m_Data, NetworkPool);

        ProjectilePool InitializePool()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
            var pool = new ProjectilePool(m_Data.ViewPrefab);
            return pool;
        }

        NetworkProjectilePool InitializeNetworkPool()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
            var pool = new NetworkProjectilePool(m_Data.NetworkViewPrefab);
            return pool;
        }

        void OnSceneChanged(Scene scene1, Scene scene2)
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            m_Pool?.ClearPool();
            m_Pool = null;
            m_NetworkPool?.ClearPool();
            m_NetworkPool = null;
        }
    }
}