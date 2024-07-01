using UnityEngine;

namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "ExplosionData", menuName = "ScriptableObjects/Effects/ExplosionDataSO")]
    public class ExplosionDataSO : EffectDataSO
    {
        [SerializeField]
        ParticleSystem m_ParticleSystemPrefab;
        public ParticleSystem ParticleSystemPrefab => m_ParticleSystemPrefab;
    }

}
