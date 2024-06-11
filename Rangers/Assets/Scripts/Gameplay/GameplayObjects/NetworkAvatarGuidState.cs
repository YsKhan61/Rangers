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
    public class NetworkAvatarGuidState : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkVariable<NetworkGuid> n_EntityNetworkGuid = new NetworkVariable<NetworkGuid>();

        [SerializeField]
        EntityDataContainerSO m_EntityDataContainer;

        EntityDataSO m_EntityData;

        public EntityDataSO RegisteredEntityData
        {
            get
            {
                if (m_EntityData == null)
                {
                    RegisterEntity(n_EntityNetworkGuid.Value.ToGuid());
                }

                return m_EntityData;
            }
        }

        public void SetRandomEntity()
        {
            n_EntityNetworkGuid.Value = m_EntityDataContainer.GetRandomTankData().Guid.ToNetworkGuid();
        }

        void RegisterEntity(Guid guid)
        {
            if (guid.Equals(Guid.Empty))
            {
                // not a valid Guid
                return;
            }

            // based on the Guid received, Avatar is fetched from AvatarRegistry
            if (!m_EntityDataContainer.TryGetTankData(guid, out EntityDataSO entityData))
            {
                Debug.LogError("Avatar not found!");
                return;
            }

            if (m_EntityData != null)
            {
                // already set, this is an idempotent call, we don't want to Instantiate twice
                return;
            }

            m_EntityData = entityData;

            /*if (TryGetComponent<ServerCharacter>(out ServerCharacter serverCharacter))
            {
                serverCharacter.CharacterClass = avatar.CharacterClass;
            }*/
        }
    }

}
