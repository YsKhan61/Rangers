using BTG.Utilities;
using System;
using UnityEngine;

namespace BTG.Entity
{
    /// <summary>
    /// This ScriptableObject will be the container for all possible Entity Data in the project.
    /// </summary>
    [CreateAssetMenu(fileName = "EntityDataContainer", menuName = "ScriptableObjects/EntityDataContainerSO")]
    public class EntityDataContainerSO : GuidContainerSO<EntityDataSO>
    {
        /// <summary>
        /// Get the EntityData by the SeatIndex.
        /// </summary>
        public EntityDataSO GetEntityDataBySeatIndex(int seatIndex)
        {
            return Array.Find(DataList, tankData => tankData.CharSelectSeatIndex == seatIndex);
        }


        /// <summary>
        /// Get the EntityData by the Tag.
        /// </summary>
        public EntityDataSO GetEntityData(TagSO tag)
        {
            return Array.Find(DataList, tankData => tankData.Tag == tag);
        }
    }
}

