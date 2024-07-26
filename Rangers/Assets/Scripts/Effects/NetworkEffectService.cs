using BTG.Events;
using BTG.Utilities.EventBus;
using UnityEngine;
using Unity.Netcode;
using System;
using BTG.Utilities;
using VContainer;
using BTG.Factory;



namespace BTG.Effects
{
    /// <summary>
    /// This service is responsible for handling all the effects in the multiplayer scene.
    /// </summary>
    internal class NetworkEffectService : NetworkBehaviour
    {
        private EventBinding<NetworkEffectEventData> m_EffectEventBinding;

        [Inject]
        private EffectFactoryContainerSO m_EffectFactoryContainer;

        public override void OnNetworkSpawn()
        {
            m_EffectEventBinding = new EventBinding<NetworkEffectEventData>(OnEffectEvent);
            EventBus<NetworkEffectEventData>.Register(m_EffectEventBinding);
        }

        public override void OnNetworkDespawn()
        {
            EventBus<NetworkEffectEventData>.Unregister(m_EffectEventBinding);
        }

        private void OnEffectEvent(NetworkEffectEventData data)
        {
            ClientRpcParams clientRpcParams = default;

            if (data.OwnerClientOnly)
            {
                clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { data.OwnerClientId }
                    }
                };
            }

            InvokeEffect_ClientRpc(data, clientRpcParams);
        }

        [ClientRpc]
        private void InvokeEffect_ClientRpc(NetworkEffectEventData data, ClientRpcParams clientRpcParams = default)
        {
            Guid guid = data.TagNetworkGuid.ToGuid();

            if (!TryGetFactory(guid, out EffectFactorySO factory))
            {
                return;
            }

            if (!TryGetEffect(factory, out EffectView effect))
            {
                return;
            }

            NetworkObject objectToFollow = null;

            if (data.FollowNetworkObject && TryGetNetworkObject(data.FollowNetowrkObjectId, out objectToFollow))
            {
                effect.transform.SetParent(objectToFollow.transform, Vector3.zero, Quaternion.identity);
            }
            else
            {
                effect.transform.SetPositionAndRotation(data.EffectPosition, Quaternion.identity);
            }

            effect.SetDuration(data.Duration);
            effect.Play();

            if (factory.Data.HasAudio)
            {
                EventBus<AudioEventData>.Invoke(new AudioEventData
                {
                    Tag = factory.Tag,
                    FollowTarget = data.FollowNetworkObject ? objectToFollow.transform : null,
                    Position = data.EffectPosition
                });
            }
        }

        

        private bool TryGetFactory(Guid guid, out EffectFactorySO factory)
        {
            factory = m_EffectFactoryContainer.GetFactory(guid) as EffectFactorySO;
            if (factory == null)
            {
                Debug.LogError($"No factory found for effect guid {guid}");
                return false;
            }
            return true;
        }

        private bool TryGetEffect(FactorySO<EffectView> factory, out EffectView effect)
        {
            effect = factory.GetItem();
            if (effect == null)
            {
                Debug.LogError("Failed to get effect from factory.");
                return false;
            }
            return true;
        }

        private bool TryGetNetworkObject(ulong networkObjectId, out NetworkObject networkObject)
        {
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out networkObject))
            {
                return true;
            }
            Debug.LogError($"Failed to find network object with ID {networkObjectId}");
            return false;
        }
    }

}