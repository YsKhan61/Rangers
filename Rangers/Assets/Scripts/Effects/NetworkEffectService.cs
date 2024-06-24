using BTG.Events;
using BTG.Utilities.EventBus;
using UnityEngine;
using Unity.Netcode;
using System;
using BTG.Utilities;
using VContainer;


namespace BTG.Effects
{
    public class NetworkEffectService : NetworkBehaviour
    {
        public struct NetworkEffectEvent : INetworkSerializable
        {
            public NetworkGuid EffectTagGuid;
            public Vector3 EffectPosition;

            // Serialize the data
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref EffectTagGuid);
                serializer.SerializeValue(ref EffectPosition);
            }

            // Additional constructor for easier creation
            public NetworkEffectEvent(TagSO effectTag, Vector3 effectPosition)
            {
                EffectTagGuid = effectTag.Guid.ToNetworkGuid();
                EffectPosition = effectPosition;
            }
        }

        private EventBinding<EffectEvent> m_EffectEventBinding;

        [Inject]
        private EffectFactoryContainerSO m_EffectFactoryContainer;

        public override void OnNetworkSpawn()
        {
            m_EffectEventBinding = new EventBinding<EffectEvent>(OnEffectEvent);
            EventBus<EffectEvent>.Register(m_EffectEventBinding);
        }

        public override void OnNetworkDespawn()
        {
            EventBus<EffectEvent>.Unregister(m_EffectEventBinding);
        }

        private void OnEffectEvent(EffectEvent effectEvent)
        {
            InvokeEffect_ClientRpc(new NetworkEffectEvent(effectEvent.EffectTag, effectEvent.EffectPosition));
        }

        [ClientRpc]
        private void InvokeEffect_ClientRpc(NetworkEffectEvent data)
        {
            Guid guid = data.EffectTagGuid.ToGuid();
            EffectFactorySO factory = m_EffectFactoryContainer.GetFactory(guid) as EffectFactorySO;
            if (factory == null)
            {
                Debug.LogError($"No factory found for effect tag {guid}");
                return;
            }

            EffectView effect = factory.GetItem();
            effect.transform.SetPositionAndRotation(data.EffectPosition, Quaternion.identity);
            effect.Play();
        }
    }

}