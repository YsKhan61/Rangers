using BTG.Events;
using BTG.Utilities.EventBus;
using UnityEngine;
using Unity.Netcode;
using System;
using BTG.Utilities;
using VContainer;


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
            ExplosionFactorySO factory = m_EffectFactoryContainer.GetFactory(guid) as ExplosionFactorySO;
            if (factory == null)
            {
                Debug.LogError($"No factory found for effect guid {guid}");
                return;
            }

            EffectView effect = factory.GetItem();
            
            if (data.FollowNetworkObject)
            {
                NetworkObject objectToFollow = NetworkManager.Singleton.SpawnManager.SpawnedObjects[data.FollowNetowrkObjectId];
                effect.transform.SetParent(objectToFollow.transform, Vector3.zero, Quaternion.identity);
            }
            else
            {
                effect.transform.SetPositionAndRotation(data.EffectPosition, Quaternion.identity);
            }

            effect.Play();
        }
    }

}