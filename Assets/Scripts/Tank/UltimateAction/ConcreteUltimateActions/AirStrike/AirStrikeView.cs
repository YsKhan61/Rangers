using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class AirStrikeView : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem m_ParticleSystem;

        [SerializeField]
        private AudioSource m_AudioSource;

        public void PlayParticleSystem()
        {
            m_ParticleSystem.Play();
        }

        public void PlayAudio()
        {
            m_AudioSource.Play();
        }

        public void StopParticleSystem()
        {
            m_ParticleSystem.Stop();
        }

        public void StopAudio()
        {
            m_AudioSource.Stop();
        }
    }
    
}
