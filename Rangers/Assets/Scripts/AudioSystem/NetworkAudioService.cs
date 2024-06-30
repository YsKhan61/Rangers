using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using System;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace BTG.AudioSystem
{
    internal class NetworkAudioService : NetworkBehaviour
    {
        private EventBinding<NetworkAudioEventData> m_AudioEventBinding;

        [Inject]
        private AudioDataContainerSO m_AudioDataContainer;
        private AudioPool m_Pool;

        public override void OnNetworkSpawn()
        {
            m_Pool = new AudioPool();
            m_AudioEventBinding = new EventBinding<NetworkAudioEventData>(OnAudioEvent);
            EventBus<NetworkAudioEventData>.Register(m_AudioEventBinding);
        }

        public override void OnNetworkDespawn()
        {
            EventBus<NetworkAudioEventData>.Unregister(m_AudioEventBinding);
            m_Pool.ClearPool();
        }

        private void OnAudioEvent(NetworkAudioEventData data)
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

            InvokeAudio_ClientRpc(data, clientRpcParams);
        }


        [ClientRpc]
        private void InvokeAudio_ClientRpc(NetworkAudioEventData data, ClientRpcParams clientRpcParams = default)
        {
            Guid guid = data.AudioTagNetworkGuid.ToGuid();
            AudioView view = m_Pool.GetAudioView();

            if (!m_AudioDataContainer.TryGetAudioData(guid, out AudioDataSO dataSO))
            {
                return;
            }

            if (data.FollowNetworkObject)
            {
                SetNetworkObjectAsParent(view.transform, data.FollowNetowrkObjectId);
            }
            else
            {
                view.transform.SetPositionAndRotation(data.Position, Quaternion.identity);
            }

            view.Play(dataSO, data);
        }

        private void SetNetworkObjectAsParent(UnityEngine.Transform transform, ulong networkObjectId)
        {
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject networkObject))
            {
                transform.SetParent(networkObject.transform, Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.Assert(false, $"Network object with id {networkObjectId} not found.");
            }
        }
    }
}
