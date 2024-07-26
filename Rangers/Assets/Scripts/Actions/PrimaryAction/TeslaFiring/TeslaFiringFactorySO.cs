using UnityEngine;
using UnityEngine.SceneManagement;


namespace BTG.Actions.PrimaryAction
{

    [CreateAssetMenu(fileName = "Tesla Firing Factory", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/TeslaFiringFactorySO")]
    public class TeslaFiringFactorySO : PrimaryActionFactorySO
    {
        [SerializeField]
        TeslaFiringDataSO m_Data;

        TeslaBallPool m_Pool;
        TeslaBallPool Pool => m_Pool ??= InitializePool();

        NetworkTeslaBallPool m_NetworkPool;
        NetworkTeslaBallPool NetworkPool => m_NetworkPool ??= InitializeNetworkPool();


        public override IPrimaryAction GetItem()
        {
            TeslaFiring tf = new (m_Data, Pool);
            return tf;
        }

        public override IPrimaryAction GetNetworkItem()
        {
            NetworkTeslaFiring tf = new(m_Data, NetworkPool);
            return tf;
        }

        TeslaBallPool InitializePool()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            var pool = new TeslaBallPool(m_Data.TeslaBallViewPrefab);
            return pool;
        }

        NetworkTeslaBallPool InitializeNetworkPool()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            var pool = new NetworkTeslaBallPool(m_Data.NetworkTeslaBallViewPrefab);
            return pool;
        }

        void OnActiveSceneChanged(Scene current, Scene next)
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;

            m_Pool?.ClearPool();
            m_Pool = null;
            m_NetworkPool?.ClearPool();
            m_NetworkPool = null;
        }
    }
}

