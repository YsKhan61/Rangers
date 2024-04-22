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


        private void Start()
        {
            InitializePlayerService();
            InitializeEnemyService();
        }
       

        private void InitializePlayerService()
        {
            m_PlayerService.Initialize();
        }


        private void InitializeEnemyService()
        {
            m_EnemyService.Initialize();
        }
    }

}