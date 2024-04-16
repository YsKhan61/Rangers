using BTG.Enemy;
using BTG.Entity;
using BTG.Player;
using BTG.Tank;
using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BTG.Services
{
    public class DependencyProviderService : MonoBehaviour, IDependencyProviderForOthers
    {
        [SerializeField]
        EntityFactoryContainerSO m_EntityFactoryContainer;

        [Provide]
        public EntityFactoryContainerSO ProvideEntityFactoryContainer() => m_EntityFactoryContainer;



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



        [SerializeField]
        PlayerVirtualCamera m_pvc;

        [Provide]
        public PlayerVirtualCamera ProvidePlayerVirtualCamera() => m_pvc;
    }
}
