using BTG.Utilities;
using System.Threading;
using UnityEngine;

namespace BTG.Effects
{

    public class ExplosionView : EffectView
    {
        private ParticleSystem m_ParticleSystem;
        private AudioSource m_AudioSource;
        private ExplosionEffectPool m_Pool;
        private int m_Duration;

        private CancellationTokenSource m_CTS;

        internal void Initialize(ParticleSystem particleSystem, AudioSource source, ExplosionEffectPool pool)
        {
            m_ParticleSystem = particleSystem;
            m_AudioSource = source;
            m_Pool = pool;
            m_Duration = (int)m_ParticleSystem.main.duration;

            m_CTS = new ();
        }

        private void OnDestroy()
        {
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_CTS);
        }

        public void SetDuration(int duration) => m_Duration = duration;

        public override void Play()
        {
            m_ParticleSystem.Play();
            m_AudioSource.Play();

            _ = HelperMethods.InvokeAfterAsync(m_Duration, () => Stop(), m_CTS.Token); 
        }

        public void Stop()
        {
            m_ParticleSystem.Stop();
            m_AudioSource.Stop();
            m_Pool.ReturnExplosionEffect(this);
        }
    }

}
