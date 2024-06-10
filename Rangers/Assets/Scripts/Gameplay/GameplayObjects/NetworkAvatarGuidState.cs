using BTG.Tank;
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
        TankDataContainerSO m_TankDataContainer;

        TankDataSO m_Avatar;

        public TankDataSO RegisteredAvatar
        {
            get
            {
                if (m_Avatar == null)
                {
                    RegisterAvatar(n_EntityNetworkGuid.Value.ToGuid());
                }

                return m_Avatar;
            }
        }

        public void SetRandomAvatar()
        {
            n_EntityNetworkGuid.Value = m_TankDataContainer.GetRandomTankData().Guid.ToNetworkGuid();
        }

        void RegisterAvatar(Guid guid)
        {
            if (guid.Equals(Guid.Empty))
            {
                // not a valid Guid
                return;
            }

            // based on the Guid received, Avatar is fetched from AvatarRegistry
            if (!m_TankDataContainer.TryGetTankData(guid, out TankDataSO avatar))
            {
                Debug.LogError("Avatar not found!");
                return;
            }

            if (m_Avatar != null)
            {
                // already set, this is an idempotent call, we don't want to Instantiate twice
                return;
            }

            m_Avatar = avatar;

            /*if (TryGetComponent<ServerCharacter>(out ServerCharacter serverCharacter))
            {
                serverCharacter.CharacterClass = avatar.CharacterClass;
            }*/
        }
    }

}
