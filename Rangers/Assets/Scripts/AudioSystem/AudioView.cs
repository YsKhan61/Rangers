using BTG.Events;
using BTG.Utilities;
using System.Threading;
using UnityEngine;

namespace BTG.AudioSystem
{
    /// <summary>
    /// This class is responsible for playing audio clips at random positions in the scene
    /// The AudioPool is used to store, create and get the audio views
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    internal class AudioView : MonoBehaviour
    {
        private AudioSource m_AudioSource;
        public AudioSource AudioSource => m_AudioSource;

        private AudioPool m_Pool;
        private CancellationTokenSource m_Cts;

        private void Awake()
        {
            m_Cts = new CancellationTokenSource();
            m_AudioSource = (AudioSource)gameObject.GetOrAddComponent<AudioSource>();
            m_AudioSource.playOnAwake = false;
            m_AudioSource.loop = false;
            m_AudioSource.spatialBlend = 1;
        }

        private void OnDestroy()
        {
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_Cts);
        }

        /*/// <summary>
        /// Play once the audio clip
        /// And return the audio view to the pool after the clip is played
        /// Play the audio clip at the given position
        /// </summary>
        public void PlayOneShot(AudioClip clip, Vector3 position)
        {
            transform.position = position;
            m_AudioSource.PlayOneShot(clip);

            _ = HelperMethods.InvokeAfterAsync((int)clip.length , () =>
            {
                m_Pool.ReturnAudio(this);
            }, m_Cts.Token);
        }*/

        internal void Play(AudioDataSO data, AudioEventData eventData)
        {
            m_AudioSource.clip = data.Clip;
            m_AudioSource.loop = data.Loop;
            m_AudioSource.spatialBlend = data.SpatialBlend;
            transform.position = eventData.Position;
            m_AudioSource.Play();

            int duration = data.Loop ? data.Duration : (int)data.Clip.length;

            _ = HelperMethods.InvokeAfterAsync(duration, () =>
            {
                m_Pool.ReturnAudio(this);
            }, m_Cts.Token);
        }

        /// <summary>
        /// Show the audio view
        /// unmute the audio source
        /// </summary>
        internal void Show()
        {
            gameObject.SetActive(true);
            m_AudioSource.mute = false;
        }

        /// <summary>
        /// Hide the audio view
        /// Mute the audio source
        /// </summary>
        internal void Hide()
        {
            m_AudioSource.Stop();
            gameObject.SetActive(false);
            m_AudioSource.mute = true;
        }

        /// <summary>
        /// Set the pool for the audio view
        /// </summary>
        internal void SetPool(AudioPool pool) => m_Pool = pool;
    }
}
