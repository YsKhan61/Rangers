using BTG.Enemy;
using BTG.Player;
using BTG.Tank;
using BTG.UI;
using BTG.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace BTG.Game
{
    public class GameService : MonoBehaviour
    {
        [SerializeField] private int m_PlayerLayer;
        [SerializeField] private int m_EnemyLayer;
        
        [SerializeField] private IntDataSO m_TestPlayerTankID; // This is for test purpose, so that we dont have to go to MainMenu and then select the tank
        [SerializeField] private WaveConfigSO m_EnemyWaves;

        [SerializeField] TankDataContainerSO m_TankDataList;
        [SerializeField] PlayerVirtualCamera m_PVCController;

        [SerializeField] private UltimateUI m_UltimateUI;
        [SerializeField] private HealthUI m_HealthUI;

        [SerializeField] private EnemyDataSO m_EnemyData;

        //cache
        private PlayerService m_PlayerService;

        private void Start()
        {
            CreateTankFactory(out TankFactory tankFactory);

            // InitializePlayerService(tankFactory);

            InitializeEnemyService(tankFactory, m_EnemyData); 
        }

        private void CreateTankFactory(out TankFactory tankFactory)
        {
            tankFactory = new TankFactory(m_TankDataList);
        }

        private void InitializePlayerService(in TankFactory tankFactory)
        {
            int tankId = m_TestPlayerTankID > 0 ? m_TestPlayerTankID : PlayerPrefs.GetInt("TankID", 1);     // test purpose

            m_PlayerService = new PlayerService(
                tankId,
                tankFactory,
                m_PVCController,
                m_UltimateUI,
                m_HealthUI,
                m_PlayerLayer,
                m_EnemyLayer);

            m_PlayerService.Initialize();
        }

        private void InitializeEnemyService(TankFactory tankFactory, EnemyDataSO enemyData)
        {
            EnemyService enemyService = new EnemyService(tankFactory, m_EnemyWaves, m_PlayerLayer, m_EnemyLayer);
            enemyService.CreateEnemyPool(m_EnemyData);
            enemyService.StartNextWave();
        }
    }
}