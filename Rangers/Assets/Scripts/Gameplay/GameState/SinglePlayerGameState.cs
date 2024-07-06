using BTG.Enemy;
using BTG.Player;
using BTG.UnityServices.Auth;
using BTG.Utilities;
using UnityEngine;
using VContainer;


namespace BTG.Gameplay.GameState
{
    public class SinglePlayerGameState : GameStateBehaviour
    {
        [SerializeField]
        PlayerVirtualCamera m_PVC;

        [Inject]
        PlayerStatsSO m_PlayerStatsData;

        [Inject]
        AuthenticationServiceFacade _authServiceFacade;

        PlayerService m_PlayerService;
        EnemyService m_EnemyService;

        public override GameState ActiveState => GameState.SinglePlay;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.Register<PlayerService>(Lifetime.Singleton);
            builder.Register<EnemyService>(Lifetime.Singleton);
        }

        protected override void Start()
        {
            base.Start();

            m_PlayerService = Container.Resolve<PlayerService>();
            m_EnemyService = Container.Resolve<EnemyService>();

            InitializePlayerService();
            InitializeEnemyService();

            GetPlayerName();
        }
        
        private void InitializePlayerService()
        {
            m_PlayerService.Initialize();
            m_PlayerService.SetPlayerVirtualCamera(m_PVC);
        }

        private async void GetPlayerName() => await _authServiceFacade.GetPlayerName();
        private void InitializeEnemyService() => m_EnemyService.Initialize();
    }

}