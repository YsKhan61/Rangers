using BTG.Events;
using BTG.Utilities.EventBus;
using UnityEngine;

namespace BTG.AudioSystem
{
    internal class AudioService : MonoBehaviour
    {
        [SerializeField]
        private AudioDataContainerSO m_AudioDataContainer;

        private EventBinding<AudioEventData> m_AudioEventBinding;
        private AudioPool m_Pool;

        private void OnEnable()
        {
            m_Pool = new AudioPool();
            m_AudioEventBinding = new EventBinding<AudioEventData>(PlayAudio);
            EventBus<AudioEventData>.Register(m_AudioEventBinding);
        }

        private void OnDisable()
        {
            EventBus<AudioEventData>.Unregister(m_AudioEventBinding);
            m_Pool.ClearPool();
        }

        private void PlayAudio(AudioEventData eventData)
        {
            bool found = m_AudioDataContainer.TryGetAudioData(eventData.Tag, out AudioDataSO data);
            if (!found)
            {
                return;
            }
            m_Pool.GetAudioView().Play(data, eventData);
        }
    }
}
