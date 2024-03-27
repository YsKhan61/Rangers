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
        [SerializeField] private IntDataSO m_TestPlayerTankID; // This is for test purpose, so that we dont have to go to MainMenu and then select the tank
        [SerializeField] private IntDataSO m_TestEnemyTankID; // This is for test purpose, so that we dont have to go to MainMenu and then select the enemy

        [SerializeField] TankDataContainerSO m_TankDataList;
        [SerializeField] PlayerVirualCameraController m_PVCController;

        [SerializeField] private UltimateUI m_UltimatePanel;

        //cache
        private PlayerService m_PlayerService;

        private void Start()
        {
            m_PlayerService = new PlayerService();
            TankFactory tankFactory = new TankFactory(m_TankDataList);
            int tankId = m_TestPlayerTankID > 0 ? m_TestPlayerTankID : PlayerPrefs.GetInt("TankID", 1);
            m_PlayerService.ConfigurePlayerTank(tankId, tankFactory, m_PVCController, m_UltimatePanel);
            new EnemyService(tankFactory).SpawnEnemyTank(m_TestEnemyTankID);
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