using BTG.Enemy;
using BTG.Player;
using BTG.Tank;
using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.Services
{
    public class GameService : MonoBehaviour
    {
        TankFactory m_TankFactory;
        PlayerService m_PlayerService;
        EnemyService m_EnemyService;


        private void Awake()
        {
            CreateTankFactory();
            CreatePlayerService();
            CreateEnemyService();
        }

        private void Start()
        {
            InitializeTankFactory();
            InitializePlayerService();
            InitializeEnemyService();
        }

        private void CreateTankFactory()
        {
            object obj = DIManager.Instance.ProvideType(typeof(TankFactory));
            m_TankFactory = obj as TankFactory;
        }

        private void CreatePlayerService()
        {
            object obj = DIManager.Instance.ProvideType(typeof(PlayerService)); 
            m_PlayerService = obj as PlayerService;
        }

        private void CreateEnemyService()
        {
            object obj = DIManager.Instance.ProvideType(typeof(EnemyService));
            m_EnemyService = obj as EnemyService;
        }


        private void InitializeTankFactory()
        {
            DIManager.Instance.Inject(m_TankFactory);
            m_TankFactory.Initialize();
        }
       

        private void InitializePlayerService()
        {
            DIManager.Instance.Inject(m_PlayerService);
            m_PlayerService.Initialize();
        }


        private void InitializeEnemyService()
        {
            DIManager.Instance.Inject(m_EnemyService);
            m_EnemyService.StartNextWave();
        }
    }
}