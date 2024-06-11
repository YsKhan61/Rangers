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
        /// Try to get the TankData by the Guid.
        /// </summary>
        public bool TryGetTankData(Guid guid, out EntityDataSO entityData)
        {
            entityData = System.Array.Find(m_EntityDataList, tankData => tankData.Guid == guid);

            return entityData != null;
        }


        /// <summary>
        /// Get a random TankData from the l
        public EntityDataSO GetRandomTankData()
        {
            if (m_EntityDataList == null || m_EntityDataList.Length == 0)
            {
                return null;
            }

            return m_EntityDataList[UnityEngine.Random.Range(0, m_EntityDataList.Length)];
        }


        /// <summary>
        /// Get the TankData by the seat index of the character select screen.
        /// </summary>
        public EntityDataSO GetTankDataBySeatIndex(int seatIndex)
        {
            return Array.Find(m_EntityDataList, tankData => tankData.CharSelectSeatIndex == seatIndex);
        }
    }
}

