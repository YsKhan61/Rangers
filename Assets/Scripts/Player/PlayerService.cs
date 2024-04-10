using BTG.Tank;
using BTG.Utilities;
using BTG.Utilities.DI;
using System.Threading;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerService
    {
        [Inject]
        private PlayerDataSO m_PlayerData;

        [Inject]
        private PlayerStatsSO m_PlayerStats;

        [Inject]
        private TankFactory m_TankFactory;


        private PlayerController m_PlayerController;

        private readonly PlayerVirtualCamera m_PVC;    // temporary for now

        private CancellationTokenSource m_CTS;

        public PlayerService() { }

        public void Initialize()
        {
            CreatePlayerControllerAndInput();
            m_CTS = new CancellationTokenSource();
            
            m_PlayerStats.ResetStats();

            m_PlayerStats.TankIDSelected.OnValueChanged += Respawn;
        }

        ~PlayerService()
        {
            m_PlayerStats.TankIDSelected.OnValueChanged -= Respawn;

            m_CTS.Cancel();
            m_CTS.Dispose();
        }

        public void OnEntityInitialized(Sprite icon)
        {
            m_PlayerStats.PlayerIcon.Value = icon;
        }

        public void OnPlayerDeath()
        {
            m_PlayerStats.DeathCount.Value++;
        }

        private void CreatePlayerControllerAndInput()
        {
            m_PlayerController = new PlayerController(this, m_PlayerData);
            DIManager.Instance.Inject(m_PlayerController);
            PlayerInputs playerInput = new(m_PlayerController);
            playerInput.Initialize();
        }

        private void Respawn(int _)
        {
            bool tankFound = CreateAndSpawnPlayerTank(out TankBrain tank);
            if (!tankFound)
                return;

            ConfigureTankAndController(tank);
        }

        private bool CreateAndSpawnPlayerTank(out TankBrain tank)
        {
            if (!m_TankFactory.TryGetTank(m_PlayerStats.TankIDSelected.Value, out tank))
            {
                return false;
            }

            return true;
        }

        private void ConfigureTankAndController(TankBrain tank)
        {
            m_PlayerController.Transform.position = Vector3.zero;
            m_PlayerController.Transform.rotation = Quaternion.identity;
            m_PlayerController.SetEntity(tank);
        }
    }
}

