using UnityEngine;

namespace BTG.Tank
{
    public class TankAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioSource m_EngineAudioSource;

        [SerializeField]
        private AudioSource m_ShootingAudioSource;

        public void PlayEngineIdleClip(AudioClip clip)
        {
            m_EngineAudioSource.clip = clip;
            m_EngineAudioSource.loop = true;
            m_EngineAudioSource.Play();
        }

        public void StopEngineAudio()
        {
            m_EngineAudioSource.Stop();
        }

        public void PlayEngineDrivingClip(AudioClip clip)
        {
            m_EngineAudioSource.clip = clip;
            m_EngineAudioSource.loop = true;
            m_EngineAudioSource.Play();
        }

        public void UpdateEngineDrivingClipPitch(float amount)
        {
            m_EngineAudioSource.pitch = Mathf.Lerp(0.2f, 1.2f, amount);
        }


        public void PlayChargingClip(AudioClip clip)
        {
            m_ShootingAudioSource.clip = clip;
            m_ShootingAudioSource.Play();
        }

        public void UpdateChargingClipPitch(float amount)
        {
            m_ShootingAudioSource.pitch = 0.5f + amount;
        }

        public void StopChargingClip()
        {
            m_ShootingAudioSource.Stop();
        }

        public void PlayShotFiringClip(AudioClip clip)
        {
            m_ShootingAudioSource.pitch = 1f;
            m_ShootingAudioSource.PlayOneShot(clip);
        }
    }
}

