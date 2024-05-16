using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    public class SelfShieldView : MonoBehaviour, IDamageableView
    {
        [SerializeField]
        private ParticleSystem[] m_ParticleSystems;

        [SerializeField]
        private AudioSource m_AudioSource;

        public Transform Transform => transform;

        public void Damage(int damage)
        {
            // Do nothing for now.
        }

        public void SetParticleSystem(float duration)
        {
            if (IsParticlesNull())
            {
                return;
            }

            foreach (ParticleSystem particleSystem in m_ParticleSystems)
            {
                SetParticleSystemDuration(particleSystem, duration);
            }
        }

        public void PlayParticleSystem()
        {
            if (IsParticlesNull())
            {
                return;
            }

            foreach (ParticleSystem particleSystem in m_ParticleSystems)
            {
                particleSystem.Play();
            }
        }

        public void StopParticleSystem()
        {
            if (IsParticlesNull())
            {
                return;
            }

            foreach (ParticleSystem particleSystem in m_ParticleSystems)
            {
                particleSystem.Stop();
            }
        }

        public void PlayAudio()
        {
            m_AudioSource.Play();
        }

        private void SetParticleSystemDuration(ParticleSystem particleSystem, float duration)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.duration = duration;
            mainModule.startLifetime = duration;
        }

        private bool IsParticlesNull()
        {
            if (m_ParticleSystems == null || m_ParticleSystems.Length == 0)
            {
                Debug.LogError("No particle systems assigned to the view");
                return true;
            }
            
            return false;
        }
    }
}
