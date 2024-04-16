using BTG.Enemy;
using BTG.Player;
using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.Services
{
    public class GameService : MonoBehaviour, ISelfDependencyRegister, IDependencyInjector
    {
        [Inject]
        PlayerService m_PlayerService;

        [Inject]
        EnemyService m_EnemyService;


        private void Awake()
        {
            // CreatePlayerService();
            // CreateEnemyService();
        }

        private void Start()
        {
            InitializePlayerService();
            InitializeEnemyService();
        }

        private void CreatePlayerService()
        {
            object obj = DIManager.Instance.RegisterType(typeof(PlayerService)); 
            m_PlayerService = obj as PlayerService;
        }

        private void CreateEnemyService()
        {
            object obj = DIManager.Instance.RegisterType(typeof(EnemyService));
            m_EnemyService = obj as EnemyService;
        }
       

        private void InitializePlayerService()
        {
            // DIManager.Instance.Inject(m_PlayerService);
            m_PlayerService.Initialize();
        }


        private void InitializeEnemyService()
        {
            // DIManager.Instance.Inject(m_EnemyService);
            m_EnemyService.StartNextWaveWithEntityTags();
        }
    }

}