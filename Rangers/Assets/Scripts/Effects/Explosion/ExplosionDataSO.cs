using UnityEngine;

namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "ExplosionData", menuName = "ScriptableObjects/Effects/ExplosionDataSO")]
    public class ExplosionDataSO : ScriptableObject
    {
        [SerializeField]
        ParticleSystem m_ParticleSystemPrefab;
        public ParticleSystem ParticleSystemPrefab => m_ParticleSystemPrefab;

        [SerializeField]
        AudioClip m_AudioClip;
        public AudioClip AudioClip => m_AudioClip;
    }

}
