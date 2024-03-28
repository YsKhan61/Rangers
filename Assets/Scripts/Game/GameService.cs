using BTG.Enemy;
using BTG.Player;
using BTG.Tank;
using BTG.UI;
using BTG.Utilities;
using UnityEngine;

namespace BTG.Game
{
    public class GameService : MonoBehaviour
    {
        [SerializeField] private int m_PlayerLayer;
        [SerializeField] private int m_EnemyLayer;
        
        [SerializeField] private IntDataSO m_TestPlayerTankID; // This is for test purpose, so that we dont have to go to MainMenu and then select the tank
        [SerializeField] private WaveConfigSO m_EnemyWaves;

        [SerializeField] TankDataContainerSO m_TankDataList;
        [SerializeField] PlayerVirualCameraController m_PVCController;

        [SerializeField] private UltimateUI m_UltimatePanel;

        //cache
        private PlayerService m_PlayerService;

        private void Start()
        {
            m_PlayerService = new PlayerService();
            TankFactory tankFactory = new TankFactory(m_TankDataList);
            
            int tankId = m_TestPlayerTankID > 0 ? m_TestPlayerTankID : PlayerPrefs.GetInt("TankID", 1);     // test purpose
            m_PlayerService.SpawnPlayerTank(tankId, tankFactory, m_PVCController, m_UltimatePanel, m_PlayerLayer, m_EnemyLayer);
            
            new EnemyService(tankFactory, m_EnemyWaves, m_PlayerLayer, m_EnemyLayer).StartNextWave();
        }

        private void Update()
        {
            m_PlayerService.Update();
        }

        private void OnDestroy()
        {
            m_PlayerService.OnDestroy();
            m_PlayerService = null;
        }
    }
}