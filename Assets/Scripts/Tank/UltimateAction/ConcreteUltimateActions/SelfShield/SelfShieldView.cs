using UnityEngine;

namespace BTG.Tank.UltimateAction
{
    public class SelfShieldView : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem m_ParticleSystem;

        [SerializeField]
        private AudioSource m_AudioSource;

        public void SetParticleSystem(float duration)
        {
            ParticleSystem.MainModule mainModule = m_ParticleSystem.main;
            mainModule.duration = duration;
            mainModule.startLifetime = duration;
        }

        public void PlayParticleSystem()
        {
            m_ParticleSystem.Play();
        }

        public void StopParticleSystem()
        {
            m_ParticleSystem.Stop();
        }

        public void PlayAudio()
        {
            m_AudioSource.Play();
        }
    }
}
