using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// The view object for the Invisibility ultimate action.
    /// It contains the particle systems and audio sources for the action.
    /// </summary>
    public class InvisibilityView : MonoBehaviour
    {
        [SerializeField, Tooltip("Audio source to be played when disappearing!")]
        private AudioSource m_AudioSource;

        [SerializeField, Tooltip("Particle system to be played when disappearing!")]
        private ParticleSystem m_DisappearPS;

        [SerializeField, Tooltip("Particle system to be played when appearing!")]
        private ParticleSystem m_AppearPS;

        [SerializeField, Tooltip("Audio clip to be played when disappearing!")]
        private AudioClip m_DisappearAudioClip;

        [SerializeField, Tooltip("Audio clip to be played when appearing!")]
        private AudioClip m_AppearAudioClip;

        /// <summary>
        /// Duration of the appear particle system.
        /// </summary>
        public float AppearPSDuration => m_AppearPS.main.duration;

        /// <summary>
        /// Play the disappear particle system.
        /// </summary>
        public void PlayDisappearPS() => m_DisappearPS.Play();


        /// <summary>
        /// Play the appear particle system.
        /// </summary>
        public void PlayAppearPS() => m_AppearPS.Play();

        /// <summary>
        /// Stop the disappear particle system.
        /// </summary>
        public void StopDisappearPS() => m_DisappearPS.Stop();

        /// <summary>
        /// Stop the appear particle system.
        /// </summary>
        public void StopAppearPS() => m_AppearPS.Stop();

        /// <summary>
        /// Play the disappear audio.
        /// </summary>
        public void PlayDisappearAudio()
            => m_AudioSource.PlayOneShot(m_DisappearAudioClip);

        /// <summary>
        /// Play the appear audio.
        /// </summary>
        public void PlayAppearAudio()
            => m_AudioSource.PlayOneShot(m_AppearAudioClip);
    }
}

