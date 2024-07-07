using UnityEngine;
using UnityEngine.Serialization;

namespace BTG.Entity.Tank
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
            m_EngineAudioSource.pitch = 0.3f;
            m_EngineAudioSource.Play();
        }

        public void StopEngineAudio()
        {
            if (m_EngineAudioSource != null && m_EngineAudioSource.isPlaying)
                m_EngineAudioSource.Stop();
        }

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
    }
}

