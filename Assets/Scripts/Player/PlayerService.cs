using BTG.Tank;
using BTG.UI;
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

        private readonly UltimateUI m_UltimateUI;    // temporary for now
        private readonly HealthUI m_HealthUI;    // temporary for now
        private readonly PlayerVirtualCamera m_PVC;    // temporary for now

        private CancellationTokenSource m_CTS;

        

        public PlayerService(
            // IntDataSO tankIDSelectedData,
            PlayerVirtualCamera pvc,
            UltimateUI ultimateUI,
            HealthUI healthUI)
        {
            // m_PlayerTankIDSelectedData = tankIDSelectedData;
            m_PVC = pvc;
            m_UltimateUI = ultimateUI;
            m_HealthUI = healthUI;
        }

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
            m_PlayerController.SetTank(tank);

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

