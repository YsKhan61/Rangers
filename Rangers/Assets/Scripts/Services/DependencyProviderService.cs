using BTG.Actions.PrimaryAction;
using BTG.Actions.UltimateAction;
using BTG.Effects;
using BTG.Enemy;
using BTG.Entity;
using BTG.Player;
using BTG.Tank;
using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;
using UnityEngine.Serialization;


namespace BTG.Services
{
    /// <summary>
    /// This service provides the dependencies to other services such as class instances and scriptable objects.
    /// </summary>
    public class DependencyProviderService : MonoBehaviour, IDependencyProviderForOthers
    {
        [Header("Factory Containers")]

        [Space(5)]


        [SerializeField]
        private EntityFactoryContainerSO m_EntityFactoryContainer;
        [Provide]
        public EntityFactoryContainerSO ProvideEntityFactoryContainer() => m_EntityFactoryContainer;


        [SerializeField]
        private PrimaryActionFactoryContainerSO m_PrimaryActionFactoryContainer;
        [Provide]
        public PrimaryActionFactoryContainerSO ProvidePrimaryActionFactoryContainer() => m_PrimaryActionFactoryContainer;


        [SerializeField]
        private UltimateActionFactoryContainerSO m_UltimateActionFactoryContainer;
        [Provide]
        public UltimateActionFactoryContainerSO ProvideUltimateActionFactoryContainer() => m_UltimateActionFactoryContainer;


        [SerializeField]
        private EnemyTankUltimateStateFactoryContainerSO m_EnemyTankUltimateStateFactoryContainer;
        [Provide]
        public EnemyTankUltimateStateFactoryContainerSO ProvideEnemyTankUltimateStateFactoryContainer() => m_EnemyTankUltimateStateFactoryContainer;


        [SerializeField]
        private RagdollFactoryContainerSO m_RagdollFactoryContainer;
        [Provide]
        public RagdollFactoryContainerSO ProvideRagdollFactoryContainer() => m_RagdollFactoryContainer;


        [Space(10)]



        [Header("Data Containers")]

        [Space(5)]


        [SerializeField]
        private PlayerDataSO m_PlayerData;
        [Provide]
        public PlayerDataSO ProvidePlayerData() => m_PlayerData;


        [SerializeField]
        private EnemyDataSO m_EnemyData;
        [Provide]
        public EnemyDataSO ProvideEnemyData() => m_EnemyData;
        

        [SerializeField]
        private PlayerStatsSO m_PlayerStats;
        [Provide]
        public PlayerStatsSO ProvidePlayerStats() => m_PlayerStats;
        [Provide]
        public IntDataSO ProvideEliminatedEnemiesCount() => m_PlayerStats.EliminatedEnemiesCount;

        [FormerlySerializedAs("m_TankDataList")]
        [SerializeField]
        private EntityDataContainerSO m_EntityDataContainer;
        [Provide]
        public EntityDataContainerSO ProvideEntityDataList() => m_EntityDataContainer;


        [SerializeField]
        private WaveConfigSO m_EnemyWaves;
        [Provide]
        public WaveConfigSO ProvideEnemyWaves() => m_EnemyWaves;


        [SerializeField]
        private PlayerVirtualCamera m_pvc;
        [Provide]
        public PlayerVirtualCamera ProvidePlayerVirtualCamera() => m_pvc;
    }
}
