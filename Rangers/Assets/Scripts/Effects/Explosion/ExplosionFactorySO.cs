using BTG.Factory;
using UnityEngine;


namespace BTG.Effects
{
    /// <summary>
    /// A factory to create the explosion effect depending on data.
    /// </summary>
    [CreateAssetMenu(fileName = "Explosion Factory", menuName = "ScriptableObjects/Factory/Effects Factory/ExplosionFactorySO")]
    public class ExplosionFactorySO : FactorySO<EffectView>
    {
        [SerializeField]
        private ExplosionDataSO m_Data;

        private ExplosionEffectPool m_Pool;
        public ExplosionEffectPool Pool => m_Pool ??= new ExplosionEffectPool(m_Data);

        public override EffectView GetItem() => Pool.GetExplosionEffect();
    }
}
