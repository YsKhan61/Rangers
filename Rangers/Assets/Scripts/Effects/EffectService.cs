using BTG.Events;
using BTG.Factory;
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
            if (!TryGetFactory(effectEvent.EffectTag, out FactorySO<EffectView> factory))
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
                effect.transform.SetPositionAndRotation(effectEvent.EffectPosition, Quaternion.identity);
            }
            effect.Play();
        }

        private bool TryGetFactory(TagSO tag, out FactorySO<EffectView> factory)
        {
            factory = m_EffectFactoryContainer.GetFactory(tag);
            if (factory == null)
            {
                Debug.LogError($"No factory found for effect tag {tag}");
                return false;
            }
            return true;
        }
    }

}