using BTG.Utilities;
using System.Threading;
using UnityEngine;

namespace BTG.Effects
{

    public class ExplosionView : EffectView
    {
        private ParticleSystem m_ParticleSystem;
        private ExplosionDataSO m_Data;
        private ExplosionEffectPool m_Pool;
        private CancellationTokenSource m_CTS;

        internal void Initialize(
            ParticleSystem particleSystem, ExplosionEffectPool pool, ExplosionDataSO data)
        {
            gameObject.name = particleSystem.gameObject.name;
            m_ParticleSystem = particleSystem;
            m_Pool = pool;
            m_Data = data;

            m_CTS = new ();
        }

        private void OnDestroy()
        {
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_CTS);
        }

        public override void Play()
        {
            m_ParticleSystem.Play();
            _ = HelperMethods.InvokeAfterAsync(GetDuration(), () => Stop(), m_CTS.Token); 
        }

        public void Stop()
        {
            m_ParticleSystem.Stop();
            m_Pool.ReturnExplosionEffect(this);

            // Debug.Log("Explosion stopped and returned to pool." + gameObject.name);
        }

        /// <summary>
        /// If the duration is overriden, return the overriden duration.
        /// else if the duration is set in the data, return the data duration.
        /// else return the duration of the particle system.
        /// </summary>
        private int GetDuration() =>
            (overridenDuration > 0) ? 
            overridenDuration : (m_Data.Duration > 0) ? m_Data.Duration : (int) m_ParticleSystem.main.duration;
    }

}
