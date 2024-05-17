using UnityEngine;

namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "Explosion Factory", menuName = "ScriptableObjects/Factory/Effects Factory/ExplosionFactorySO")]
    public class ExplosionFactorySO : ScriptableObject
    {
        [SerializeField]
        private ExplosionDataSO m_Data;

        private ExplosionEffectPool m_Pool;
        public ExplosionEffectPool Pool => m_Pool ??= new ExplosionEffectPool(m_Data);

        public void CreateExplosion(Vector3 position)
        {
            var effect = Pool.GetExplosionEffect();
            effect.transform.position = position;
            effect.Play();
        }
    }

}
