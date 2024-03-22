using UnityEngine;

namespace BTG.Tank
{
    [CreateAssetMenu(fileName = "TankDataContainer", menuName = "ScriptableObjects/TankDataContainerSO")]
    public class TankDataContainerSO : ScriptableObject
    {
        [SerializeField] private TankDataSO[] m_TankDataList;
        public TankDataSO[] TankDataList => m_TankDataList;
    }
}

