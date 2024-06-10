using BTG.Factory;
using UnityEngine;


namespace BTG.Effects
{
    /// <summary>
    /// A factory to create the explosion effect depending on data.
    /// </summary>
    [CreateAssetMenu(fileName = "Explosion Factory", menuName = "ScriptableObjects/Factory/Effects Factory/ExplosionFactorySO")]
    public class ExplosionFactorySO : FactorySO<ExplosionView>
    {
        [SerializeField]
        private ExplosionDataSO m_Data;

        private ExplosionEffectPool m_Pool;
        public ExplosionEffectPool Pool => m_Pool ??= new ExplosionEffectPool(m_Data);

        public void CreateExplosion(Vector3 position)
        {
            var effect = GetItem();
            effect.transform.position = position;
            effect.Play();
        }

        public override ExplosionView GetItem() => Pool.GetExplosionEffect();
    }
}
