using BTG.Utilities;
using UnityEngine;

namespace BTG.AudioSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioView : MonoBehaviour
    {
        private AudioSource m_AudioSource;
        public AudioSource AudioSource => m_AudioSource;

        private AudioPool m_Pool;

        private void Awake()
        {
            m_AudioSource = (AudioSource)gameObject.GetOrAddComponent<AudioSource>();
        }

        public void SetPool(AudioPool pool) => m_Pool = pool;

        public void Show()
        {
            gameObject.SetActive(true);
            m_AudioSource.mute = false;
        }

        public void Hide()
        {
            m_AudioSource.Stop();
            gameObject.SetActive(false);
            m_AudioSource.mute = true;
        }
    }
}
