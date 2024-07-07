using BTG.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BTG.Effects
{
    /// <summary>
    /// A factory to create the explosion effect depending on data.
    /// </summary>
    [CreateAssetMenu(fileName = "Explosion Factory", menuName = "ScriptableObjects/Factory/Effects Factory/ExplosionFactorySO")]
    public class ExplosionFactorySO : EffectFactorySO
    {
        [SerializeField]
        private ExplosionDataSO m_Data;
        public override EffectDataSO Data => m_Data;

        private ExplosionEffectPool m_Pool;
        public ExplosionEffectPool Pool => m_Pool ??= InitializePool();

        public override EffectView GetItem() => Pool.GetExplosionEffect();

        ExplosionEffectPool InitializePool()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            var pool = new ExplosionEffectPool(m_Data);
            return pool;
        }

        void OnActiveSceneChanged(Scene current, Scene next)
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;

            m_Pool?.ClearPool();
            m_Pool = null;
        }
    }
}
