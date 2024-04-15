using BTG.Utilities;
using System.Threading;
using UnityEngine;

namespace BTG.Effects
{

    public class ExplosionView : MonoBehaviour, IExplosionView
    {
        private ParticleSystem m_ParticleSystem;
        private AudioSource m_AudioSource;
        private ExplosionEffectPool m_Pool;

        private CancellationTokenSource m_CTS;

        public void Initialize(ParticleSystem particleSystem, AudioSource source, ExplosionEffectPool pool)
        {
            m_ParticleSystem = particleSystem;
            m_AudioSource = source;

            m_CTS = new ();
        }

        private void OnDestroy()
        {
            HelperMethods.DisposeCancellationTokenSource(m_CTS);
        }

        public void Play()
        {
            m_ParticleSystem.Play();
            m_AudioSource.Play();

            _ = HelperMethods.InvokeAfterAsync((int)m_ParticleSystem.main.duration, () => Stop(), m_CTS.Token); 
        }

        public void Stop()
        {
            m_ParticleSystem.Stop();
            m_AudioSource.Stop();
            m_Pool.ReturnExplosionEffect(this);
        }
    }

}
