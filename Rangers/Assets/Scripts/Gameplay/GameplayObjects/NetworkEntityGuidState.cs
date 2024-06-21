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

        [SerializeField]
        EntityDataContainerSO m_EntityDataContainer;
        public EntityDataContainerSO EntityDataContainer => m_EntityDataContainer;

        EntityDataSO m_EntityData;

        public EntityDataSO RegisteredEntityData
        {
            get
            {
                if (m_EntityData == null)
                {
                    Debug.LogError("Entity Data not registered yet!");
                    return null;
                }
                
                return m_EntityData;
            }
        }

        public void SetRandomEntity()
        {
            RegisterEntity(m_EntityDataContainer.GetRandomData().Guid);
        }

        [ClientRpc]
        public void RegisterEntityData_ClientRpc(NetworkGuid networkGuid)
        {
            RegisterEntity(networkGuid.ToGuid());
        }

        private void RegisterEntity(Guid guid)
        {
            if (guid.Equals(Guid.Empty))
            {
                Debug.Log("Guid is empty! Clearing Entity Data!");
                m_EntityData = null;
                // not a valid Guid
                return;
            }

            // based on the Guid received, Avatar is fetched from AvatarRegistry
            if (!m_EntityDataContainer.TryGetData(guid, out EntityDataSO entityData))
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
