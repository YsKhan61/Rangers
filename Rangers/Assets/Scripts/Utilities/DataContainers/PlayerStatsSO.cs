using UnityEngine;



namespace BTG.Utilities
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
    public class PlayerStatsSO : ScriptableObject
    {
        [SerializeField]
        private TagDataSO m_TankTagSelected;
        public TagDataSO EntityTagSelected => m_TankTagSelected;

        [SerializeField]
        private StringDataSO m_PlayerName;
        public StringDataSO PlayerName => m_PlayerName;

        [SerializeField]
        private SpriteDataSO m_PlayerIcon;
        public SpriteDataSO PlayerIcon => m_PlayerIcon;

        [SerializeField]
        IntIntEventChannelSO m_PlayerHealthEventChannel;
        public IntIntEventChannelSO PlayerHealthEventChannel => m_PlayerHealthEventChannel;

        [SerializeField]
        private IntDataSO m_DeathCount;
        public IntDataSO DeathCount => m_DeathCount;

        [SerializeField]
        private IntDataSO m_EliminatedEnemiesCount;
        public IntDataSO EliminatedEnemiesCount => m_EliminatedEnemiesCount;

        public void ResetStats()
        {
            m_DeathCount.Value = 0;
            m_EliminatedEnemiesCount.Value = 0;
        }
    }
}