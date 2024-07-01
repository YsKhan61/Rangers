using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using UnityEngine;
using VContainer;


namespace BTG.Effects
{
    /// <summary>
    /// This service is responsible for handling all the effects in the singleplayer scene.
    /// </summary>
    public class EffectService : MonoBehaviour
    {
        private EventBinding<EffectEventData> m_EffectEventBinding;

        [Inject]
        private EffectFactoryContainerSO m_EffectFactoryContainer;

        private void OnEnable()
        {
            m_EffectEventBinding = new EventBinding<EffectEventData>(OnEffectEvent);
            EventBus<EffectEventData>.Register(m_EffectEventBinding);
        }

        private void OnDisable()
        {
            EventBus<EffectEventData>.Unregister(m_EffectEventBinding);
        }

        private void OnEffectEvent(EffectEventData effectEvent)
        {
            InvokeEffect(effectEvent);
        }

        private void InvokeEffect(EffectEventData effectEvent)
        {
            if (!TryGetFactory(effectEvent.Tag, out EffectFactorySO factory))
            {
                return;
            }

            EffectView effect = factory.GetItem();
            if (effectEvent.FollowTarget != null)
            {
                effect.transform.SetParent(effectEvent.FollowTarget, Vector3.zero, Quaternion.identity);
            }
            else
            {
                effect.transform.SetPositionAndRotation(effectEvent.Position, Quaternion.identity);
            }
            effect.SetDuration(effectEvent.Duration);
            effect.Play();

            if (factory.Data.HasAudio)
            {
                EventBus<AudioEventData>.Invoke(new AudioEventData
                {
                    Tag = effectEvent.Tag,
                    FollowTarget = effectEvent.FollowTarget,
                    Position = effectEvent.Position
                });
            }
        }

        private bool TryGetFactory(TagSO tag, out EffectFactorySO factory)
        {
            factory = m_EffectFactoryContainer.GetFactory(tag) as EffectFactorySO;
            if (factory == null)
            {
                Debug.LogError($"No factory found for effect tag {tag}");
                return false;
            }
            return true;
        }
    }

}