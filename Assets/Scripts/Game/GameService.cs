using BTG.Enemy;
using BTG.Player;
using BTG.Tank;
using BTG.UI;
using UnityEngine;

namespace BTG.Game
{
    public class GameService : MonoBehaviour
    {
        [SerializeField] private int m_TestTankID; // This is for test purpose, so that we dont have to go to MainMenu and then select the tank
        [SerializeField] private int m_TestEnemyID; // This is for test purpose, so that we dont have to go to MainMenu and then select the enemy

        [SerializeField] TankDataContainerSO m_TankDataList;
        [SerializeField] PlayerVirualCameraController m_PVCController;

        [SerializeField] private UltimateUI m_UltimatePanel;

        //cache
        private PlayerService m_PlayerService;

        private void Start()
        {
            m_PlayerService = new PlayerService();
            TankFactory tankFactory = new TankFactory(m_TankDataList);
            int tankId = PlayerPrefs.GetInt("TankID", m_TestTankID);
            m_PlayerService.ConfigurePlayer(tankId, tankFactory, m_PVCController, m_UltimatePanel);
            new EnemyService(tankFactory).ConfigureEnemy(m_TestEnemyID);
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