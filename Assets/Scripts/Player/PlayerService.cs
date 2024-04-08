using BTG.EventSystem;
using BTG.Tank;
using BTG.UI;
using BTG.Utilities;
using System.Threading;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerService
    {
        private PlayerController m_PlayerController;
        private TankFactory m_TankFactory;
        private int m_PlayerLayer;
        private int m_EnemyLayer;

        private readonly UltimateUI m_UltimateUI;    // temporary for now
        private readonly HealthUI m_HealthUI;    // temporary for now
        private readonly PlayerVirtualCamera m_PVC;    // temporary for now

        private CancellationTokenSource m_CTS;

        private PlayerDataSO m_PlayerData;
        private IntDataSO m_PlayerTankIDSelectedData;
        private PlayerStatsSO m_PlayerStats;

        public PlayerService(
            IntDataSO tankIDSelectedData,
            TankFactory tankFactory,
            PlayerVirtualCamera pvc,
            UltimateUI ultimateUI,
            HealthUI healthUI,
            int playerLayer,
            int enemyLayer,
            PlayerDataSO playerData,
            PlayerStatsSO playerStats)
        {
            m_PlayerTankIDSelectedData = tankIDSelectedData;
            m_TankFactory = tankFactory;
            m_PlayerLayer = playerLayer;
            m_EnemyLayer = enemyLayer;
            m_PVC = pvc;
            m_UltimateUI = ultimateUI;
            m_HealthUI = healthUI;
            m_PlayerData = playerData;
            m_PlayerStats = playerStats;
        }

        public void Initialize()
        {
            CreatePlayerControllerAndInput();
            m_CTS = new CancellationTokenSource();
            
            m_PlayerStats.ResetStats();

            m_PlayerTankIDSelectedData.OnValueChanged += Respawn;
        }

        ~PlayerService()
        {
            m_PlayerTankIDSelectedData.OnValueChanged -= Respawn;

            m_CTS.Cancel();
            m_CTS.Dispose();
        }

        public void OnPlayerDeath()
        {
            m_PlayerStats.DeathCount.Value++;
        }

        private void CreatePlayerControllerAndInput()
        {
            m_PlayerController = new PlayerController(this, m_PlayerData);
            PlayerInputs playerInput = new PlayerInputs(m_PlayerController);
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
            if (!m_TankFactory.TryGetTank(m_PlayerTankIDSelectedData.Value, out tank))
            {
                return false;
            }

            return true;
        }

        private void ConfigureTankAndController(TankBrain tank)
        {
            m_PlayerController.Transform.position = Vector3.zero;
            m_PlayerController.Transform.rotation = Quaternion.identity;
            m_PlayerController.SetTank(tank, m_PlayerLayer, m_EnemyLayer);

            ConfigurePlayerCameraWithTankController(tank);
            ConfigureUltimateUIWithTankController(tank);
            ConfigureHealthUIWithTankController(tank);  
        }

        private void ConfigurePlayerCameraWithTankController(TankBrain tank)
        {
            m_PVC.Initialize(tank.CameraTarget);
            tank.SubscribeToOnTankShootEvent(m_PVC.ShakeCameraOnPlayerTankShoot);
            tank.SubscribeToCameraShakeEvent(m_PVC.ShakeCameraOnUltimateExecution);
        }

        private void ConfigureUltimateUIWithTankController(TankBrain tank)
        {
            tank.SubscribeToUltimateActionAssignedEvent(m_UltimateUI.Init);
            tank.SubscribeToChargeUpdatedEvent(m_UltimateUI.UpdateChargeAmount);
            tank.SubscribeToFullyChargedEvent(m_UltimateUI.OnFullyCharged);
            tank.SubscribeToUltimateExecutedEvent(m_UltimateUI.OnUltimateExecuted);
        }

        private void ConfigureHealthUIWithTankController(TankBrain tank)
        {
            tank.SubscribeToTankInitializedEvent(m_HealthUI.SetTankIcon);
            tank.SubscribeToHealthUpdatedEvent(m_HealthUI.UpdateHealth);
        }
    }
}

