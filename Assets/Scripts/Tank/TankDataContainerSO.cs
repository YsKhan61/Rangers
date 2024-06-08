using System;
using UnityEngine;

namespace BTG.Tank
{
    /// <summary>
    /// This ScriptableObject will be the container for all possible TankData inside Rangers.
    /// </summary>
    [CreateAssetMenu(fileName = "TankDataContainer", menuName = "ScriptableObjects/TankDataContainerSO")]
    public class TankDataContainerSO : ScriptableObject
    {
        [SerializeField] private TankDataSO[] m_TankDataList;
        public TankDataSO[] TankDataList => m_TankDataList;


        /// <summary>
        /// Try to get the TankData by the Guid.
        /// </summary>
        public bool TryGetTankData(Guid guid, out TankDataSO tankDataSO)
        {
            tankDataSO = System.Array.Find(m_TankDataList, tankData => tankData.Guid == guid);

            return tankDataSO != null;
        }


        /// <summary>
        /// Get a random TankData from the l
        public TankDataSO GetRandomTankData()
        {
            if (m_TankDataList == null || m_TankDataList.Length == 0)
            {
                return null;
            }

            return m_TankDataList[UnityEngine.Random.Range(0, m_TankDataList.Length)];
        }


        /// <summary>
        /// Get the TankData by the seat index of the character select screen.
        /// </summary>
        public TankDataSO GetTankDataBySeatIndex(int seatIndex)
        {
            return Array.Find(m_TankDataList, tankData => tankData.CharSelectSeatIndex == seatIndex);
        }
    }
}

