using BTG.Enemy;
using BTG.Player;
using BTG.Tank;
using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.Services
{
    public class DependencyProviderService : MonoBehaviour, IDependencyProvider
    {
        [SerializeField]
        PlayerDataSO m_PlayerData;

        [Provide]
        public PlayerDataSO ProvidePlayerData() => m_PlayerData;


        [SerializeField]
        EnemyDataSO m_EnemyData;

        [Provide]
        public EnemyDataSO ProvideEnemyData() => m_EnemyData;


        [SerializeField]
        PlayerStatsSO m_PlayerStats;

        [Provide]
        public PlayerStatsSO ProvidePlayerStats() => m_PlayerStats;

        [Provide]
        public IntDataSO ProvideEliminatedEnemiesCount() => m_PlayerStats.EliminatedEnemiesCount;


        [SerializeField]
        TankDataContainerSO m_TankDataList;

        [Provide]
        public TankDataContainerSO ProvideTankDataList() => m_TankDataList;

        [SerializeField]
        WaveConfigSO m_EnemyWaves;

        [Provide]
        public WaveConfigSO ProvideEnemyWaves() => m_EnemyWaves;
    }
}
