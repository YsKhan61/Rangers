using BTG.Utilities;
using System;
using UnityEngine;

namespace BTG.Entity
{
    /// <summary>
    /// This ScriptableObject will be the container for all possible TankData inside Rangers.
    /// </summary>
    [CreateAssetMenu(fileName = "EntityDataContainer", menuName = "ScriptableObjects/EntityDataContainerSO")]
    public class EntityDataContainerSO : ScriptableObject
    {
        [SerializeField] private EntityDataSO[] m_EntityDataList;
        public EntityDataSO[] EntityDataList => m_EntityDataList;


        /// <summary>
        /// Try to get the EntityData by the Guid.
        /// </summary>
        public bool TryGetEntityData(Guid guid, out EntityDataSO entityData)
        {
            entityData = System.Array.Find(m_EntityDataList, tankData => tankData.Guid == guid);

            return entityData != null;
        }


        /// <summary>
        /// Get a random EntityData from the list
        public EntityDataSO GetRandomEntityData()
        {
            if (m_EntityDataList == null || m_EntityDataList.Length == 0)
            {
                return null;
            }

            return m_EntityDataList[UnityEngine.Random.Range(0, m_EntityDataList.Length)];
        }


        /// <summary>
        /// Get the EntityData by the SeatIndex.
        /// </summary>
        public EntityDataSO GetEntityDataBySeatIndex(int seatIndex)
        {
            return Array.Find(m_EntityDataList, tankData => tankData.CharSelectSeatIndex == seatIndex);
        }


        /// <summary>
        /// Get the EntityData by the Tag.
        /// </summary>
        public EntityDataSO GetEntityData(TagSO tag)
        {
            return Array.Find(m_EntityDataList, tankData => tankData.Tag == tag);
        }
    }
}

