using BTG.Events;
using BTG.Utilities.EventBus;
using BTG.Utilities;
using UnityEngine;
using VContainer;

namespace BTG.AudioSystem
{
    internal class AudioService : MonoBehaviour
    {
        [Inject]
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

            AudioView view = m_Pool.GetAudioView();
            if (eventData.FollowTarget != null)
            {
                view.transform.SetParent(eventData.FollowTarget, Vector3.zero, Quaternion.identity);
            }

            view.Play(data, eventData);
        }
    }
}
