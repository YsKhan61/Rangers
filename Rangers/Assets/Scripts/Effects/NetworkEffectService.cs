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
    public class NetworkEffectService : NetworkBehaviour
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
            Guid guid = data.EffectTagNetworkGuid.ToGuid();

            if (!TryGetFactory(guid, out FactorySO<EffectView> factory))
            {
                return;
            }

            if (!TryGetEffect(factory, out EffectView effect))
            {
                return;
            }

            if (data.FollowNetworkObject)
            {
                SetNetworkObjectAsPareant(effect.transform, data.FollowNetowrkObjectId);
            }
            else
            {
                effect.transform.SetPositionAndRotation(data.EffectPosition, Quaternion.identity);
            }

            effect.Play();
        }

        

        private bool TryGetFactory(Guid guid, out FactorySO<EffectView> factory)
        {
            factory = m_EffectFactoryContainer.GetFactory(guid);
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

        private void SetNetworkObjectAsPareant(Transform effect, ulong parentId)
        {
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(parentId, out NetworkObject objectToFollow))
            {
                effect.SetParent(objectToFollow.transform, true);
            }
            else
            {
                Debug.LogError($"Failed to find network object with ID {parentId}");
            }
        }
    }

}