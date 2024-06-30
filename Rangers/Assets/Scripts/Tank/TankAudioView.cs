using UnityEngine;
using UnityEngine.Serialization;

namespace BTG.Tank
{
    public class TankAudioView : MonoBehaviour
    {
        [SerializeField]
        private AudioSource m_EngineAudioSource;

        [SerializeField, FormerlySerializedAs("m_ShootingAudioSource")]
        private AudioSource m_PrimaryActionAudioSource;

        private AudioClip m_ChargingClip;
        private AudioClip m_ShotClip;


        public void PlayEngineIdleClip(AudioClip clip)
        {
            m_EngineAudioSource.clip = clip;
            m_EngineAudioSource.loop = true;
            m_EngineAudioSource.Play();
        }

        public void StopEngineAudio() => m_EngineAudioSource.Stop();

        public void PlayEngineDrivingClip(AudioClip clip)
        {
            m_EngineAudioSource.clip = clip;
            m_EngineAudioSource.loop = true;
            m_EngineAudioSource.Play();
        }

        /// <summary>
        /// Toggle the mute state of the engine audio.
        /// </summary>
        /// <param name="value">True - Mute, False - Unmute</param>
        public void ToggleMuteEngineAudio(bool value) => 
            m_EngineAudioSource.mute = value;

        public void ToggleMuteShootingAudio(bool value) => 
            m_PrimaryActionAudioSource.mute = value;

        public void UpdateEngineDrivingClipPitch(float amount) => 
            m_EngineAudioSource.pitch = Mathf.Lerp(0.2f, 1.2f, amount);

        public void InitializeChargingAndShootingClips(AudioClip chargingClip, AudioClip shootClip)
        {
            m_ChargingClip = chargingClip;
            m_ShotClip = shootClip;
        }

        public void PlayChargingClip()
        {
            m_PrimaryActionAudioSource.clip = m_ChargingClip;
            m_PrimaryActionAudioSource.loop = true;
            m_PrimaryActionAudioSource.Play();
        }

        public void UpdateChargingClipPitch(float amount) => 
            m_PrimaryActionAudioSource.pitch = 0.5f + amount;

        public void PlayShotFiredClip()
        {
            m_PrimaryActionAudioSource.clip = m_ShotClip;
            m_PrimaryActionAudioSource.loop = false;
            m_PrimaryActionAudioSource.Play();
        }
    }
}

