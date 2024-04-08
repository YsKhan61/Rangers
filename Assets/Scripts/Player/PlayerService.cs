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

        private int m_TankID;       // temporary for now
        private UltimateUI m_UltimateUI;    // temporary for now
        private HealthUI m_HealthUI;    // temporary for now
        private PlayerVirtualCamera m_PVC;    // temporary for now

        private CancellationTokenSource m_CTS;

        private PlayerDataSO m_PlayerData;

        public PlayerService(
            in int tankID,
            TankFactory tankFactory,
            PlayerVirtualCamera pvc,
            UltimateUI ultimateUI,
            HealthUI healthUI,
            int playerLayer,
            int enemyLayer,
            PlayerDataSO playerData)
        {
            m_TankID = tankID;
            m_TankFactory = tankFactory;
            m_PlayerLayer = playerLayer;
            m_EnemyLayer = enemyLayer;
            m_PVC = pvc;
            m_UltimateUI = ultimateUI;
            m_HealthUI = healthUI;
            m_PlayerData = playerData;
        }

        public void Initialize()
        {
            CreatePlayerControllerAndInput();
            Respawn();
            m_CTS = new CancellationTokenSource();
        }

        ~PlayerService()
        {
            m_CTS.Cancel();
            m_CTS.Dispose();
        }

        public void OnPlayerTankDead()
        {
            _ = HelperMethods.InvokeAfterAsync(3, () => Respawn(), m_CTS.Token);
        }

        private void CreatePlayerControllerAndInput()
        {
            m_PlayerController = new PlayerController(this, m_PlayerData);
            PlayerInputs playerInput = new PlayerInputs(m_PlayerController);
            playerInput.Initialize();
        }

        private void Respawn()
        {
            bool tankFound = CreateAndSpawnPlayerTank(out TankBrain tank);
            if (!tankFound)
                return;

            ConfigureTankAndController(tank);
        }

        private bool CreateAndSpawnPlayerTank(out TankBrain tank)
        {
            if (!m_TankFactory.TryGetTank(m_TankID, out tank))
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

