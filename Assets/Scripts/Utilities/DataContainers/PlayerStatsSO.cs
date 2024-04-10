using UnityEngine;



namespace BTG.Utilities
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
    public class PlayerStatsSO : ScriptableObject
    {
        [SerializeField]
        IntDataSO m_TankIDSelected;
        public IntDataSO TankIDSelected => m_TankIDSelected;

        [SerializeField]
        IntDataSO m_DeathCount;
        public IntDataSO DeathCount => m_DeathCount;

        [SerializeField]
        IntDataSO m_EliminatedEnemiesCount;
        public IntDataSO EliminatedEnemiesCount => m_EliminatedEnemiesCount;

        public void ResetStats()
        {
            m_DeathCount.Value = 0;
            m_EliminatedEnemiesCount.Value = 0;
        }
    }
}