using BTG.Enemy;
using BTG.Player;
using BTG.Tank;
using BTG.UI;
using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.Services
{
    public class GameService : MonoBehaviour
    {
        /*[SerializeField] private int m_PlayerLayer;
        [SerializeField] private int m_EnemyLayer;*/
        
        [SerializeField] private WaveConfigSO m_EnemyWaves;

        [SerializeField] PlayerVirtualCamera m_PVCController;

        [SerializeField] private UltimateUI m_UltimateUI;
        [SerializeField] private HealthUI m_HealthUI;

        [SerializeField] private EnemyDataSO m_EnemyData;
        [SerializeField] private PlayerDataSO m_PlayerData;

        [SerializeField] private IntDataSO m_TankIDSelectedData;

        private void Start()
        {
            CreateTankFactory(out TankFactory tankFactory);

            InitializePlayerService(tankFactory);

            InitializeEnemyService(tankFactory, m_EnemyData);
        }

        private void CreateTankFactory(out TankFactory tankFactory)
        {
            tankFactory = new TankFactory();
            Injector.Instance.Inject(tankFactory);
            tankFactory.Initialize();
        }

        private void InitializePlayerService(in TankFactory tankFactory)
        {
            PlayerService playerService = new PlayerService(
                m_TankIDSelectedData,
                tankFactory,
                m_PVCController,
                m_UltimateUI,
                m_HealthUI,
                /*m_PlayerLayer,
                m_EnemyLayer,*/
                m_PlayerData);

            Injector.Instance.Inject(playerService);

            playerService.Initialize();
        }

        private void InitializeEnemyService(TankFactory tankFactory, EnemyDataSO enemyData)
        {
            EnemyService enemyService = new EnemyService(
                tankFactory, 
                m_EnemyWaves, 
                /*m_PlayerLayer, 
                m_EnemyLayer,*/ 
                m_EnemyData);

            Injector.Instance.Inject(enemyService);

            enemyService.StartNextWave();
        }
    }
}