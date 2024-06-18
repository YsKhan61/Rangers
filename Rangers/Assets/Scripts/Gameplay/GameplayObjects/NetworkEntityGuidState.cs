using BTG.Entity;
using BTG.Utilities;
using System;
using Unity.Netcode;
using UnityEngine;


namespace BTG.Gameplay.GameplayObjects
{
    /// <summary>
    /// NetworkBehaviour component to send/receive GUIDs from server to clients.
    /// </summary>
    public class NetworkEntityGuidState : NetworkBehaviour
    {
        public event Action OnEntityDataRegistered;

        [HideInInspector]
        public NetworkVariable<NetworkGuid> n_NetworkEntityGuid = new NetworkVariable<NetworkGuid>();

        [SerializeField]
        EntityDataContainerSO m_EntityDataContainer;

        EntityDataSO m_EntityData;

        public EntityDataSO RegisteredEntityData
        {
            get
            {
                if (m_EntityData == null)
                {
                    RegisterEntity(n_NetworkEntityGuid.Value.ToGuid());
                }
                
                return m_EntityData;
            }
        }

        public override void OnNetworkSpawn()
        {
            n_NetworkEntityGuid.OnValueChanged += OnEntityGuidChanged;
        }

        public override void OnNetworkDespawn()
        {
            n_NetworkEntityGuid.OnValueChanged -= OnEntityGuidChanged;
        }

        public void SetRandomEntity()
        {
            n_NetworkEntityGuid.Value = m_EntityDataContainer.GetRandomEntityData().Guid.ToNetworkGuid();
        }

        private void OnEntityGuidChanged(NetworkGuid previousValue, NetworkGuid newValue)
        {
            RegisterEntity(newValue.ToGuid());
        }

        private void RegisterEntity(Guid guid)
        {
            if (guid.Equals(Guid.Empty))
            {
                Debug.LogError("Guid is empty!");
                // not a valid Guid
                return;
            }

            // based on the Guid received, Avatar is fetched from AvatarRegistry
            if (!m_EntityDataContainer.TryGetEntityData(guid, out EntityDataSO entityData))
            {
                Debug.LogError("Entity not found!");
                return;
            }

            m_EntityData = entityData;
            Debug.Log($"{OwnerClientId} : Entity registered: {RegisteredEntityData.Tag}, IsOwner: {IsOwner}");

            OnEntityDataRegistered?.Invoke();
        }
    }

}
