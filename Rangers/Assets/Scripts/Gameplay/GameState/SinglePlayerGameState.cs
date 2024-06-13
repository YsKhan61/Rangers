using BTG.Enemy;
using BTG.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace BTG.Gameplay.GameState
{
    public class SinglePlayerGameState : GameStateBehaviour
    {
        [SerializeField]
        private PlayerVirtualCamera m_PVC;

        PlayerService m_PlayerService;

        EnemyService m_EnemyService;

        public override GameState ActiveState => GameState.SinglePlay;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
        }

        protected override void Start()
        {
            base.Start();

            m_PlayerService = Container.Resolve<PlayerService>();
            m_EnemyService = Container.Resolve<EnemyService>();

            InitializePlayerService();
            InitializeEnemyService();
        }
       

        private void InitializePlayerService()
        {
            m_PlayerService.Initialize();
            m_PlayerService.SetPlayerVirtualCamera(m_PVC);
        }


        private void InitializeEnemyService()
        {
            m_EnemyService.Initialize();
        }
    }

}