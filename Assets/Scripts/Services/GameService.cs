using BTG.Enemy;
using BTG.Player;
using BTG.Tank;
using BTG.UI;
using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.Services
{
    public class GameService : MonoBehaviour
    {
        [SerializeField] PlayerVirtualCamera m_PVCController;

        [SerializeField] private UltimateUI m_UltimateUI;

        TankFactory m_TankFactory;


        private void Awake()
        {
            InitializeTankFactory();
        }

        private void Start()
        {
            InitializePlayerService();
            InitializeEnemyService();
        }


        private void InitializeTankFactory()
        {
            object obj = DIManager.Instance.ProvideType(typeof(TankFactory));
            m_TankFactory = obj as TankFactory;
            DIManager.Instance.Inject(m_TankFactory);
            m_TankFactory.Initialize();
        }
       

        private void InitializePlayerService()
        {
            PlayerService playerService = new(
                m_PVCController,
                m_UltimateUI);

            DIManager.Instance.Inject(playerService);
            playerService.Initialize();
        }


        private void InitializeEnemyService()
        {
            EnemyService enemyService = new();
            DIManager.Instance.Inject(enemyService);
            enemyService.StartNextWave();
        }
    }
}