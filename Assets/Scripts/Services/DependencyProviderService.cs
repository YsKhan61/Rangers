using BTG.Actions.UltimateAction;
using BTG.Enemy;
using BTG.Entity;
using BTG.Player;
using BTG.Tank;
using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;


namespace BTG.Services
{
    /// <summary>
    /// This service provides the dependencies to other services such as class instances and scriptable objects.
    /// </summary>
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
        UltimateActionFactoryContainerSO m_UltimateActionFactoryContainer;
        [Provide]
        public UltimateActionFactoryContainerSO ProvideUltimateActionFactoryContainer() => m_UltimateActionFactoryContainer;


        [SerializeField]
        EnemyTankUltimateStateFactoryContainerSO m_EnemyTankUltimateStateFactoryContainer;

        [Provide]
        public EnemyTankUltimateStateFactoryContainerSO ProvideEnemyTankUltimateStateFactoryContainer() => m_EnemyTankUltimateStateFactoryContainer;


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
