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
            EventService.Instance.OnBeforeAnyTankDead.AddListener(OnTankDead);
            m_CTS = new CancellationTokenSource();
        }

        ~PlayerService()
        {
            EventService.Instance.OnBeforeAnyTankDead.RemoveListener(OnTankDead);
            m_CTS.Cancel();
            m_CTS.Dispose();
        }

        private void CreatePlayerControllerAndInput()
        {
            m_PlayerController = new PlayerController(m_PlayerData);
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

            ConfigurePlayerCameraWithTankController(m_PVC, tank);
            ConfigureUltimateUIWithTankController(m_UltimateUI, tank);
            ConfigureHealthUIWithTankController(m_HealthUI, tank);
        }

        private void ConfigurePlayerCameraWithTankController(
            PlayerVirtualCamera pvc,
            TankBrain tank)
        {
            pvc.Initialize(tank.CameraTarget);
            tank.SubscribeToOnTankShootEvent(pvc.ShakeCameraOnPlayerTankShoot);
            tank.SubscribeToCameraShakeEvent(pvc.ShakeCameraOnUltimateExecution);
        }

        private void ConfigureUltimateUIWithTankController(
            UltimateUI ultimateUI,
            TankBrain tank)
        {
            tank.SubscribeToUltimateActionAssignedEvent(ultimateUI.Init);
            tank.SubscribeToChargeUpdatedEvent(ultimateUI.UpdateChargeAmount);
            tank.SubscribeToFullyChargedEvent(ultimateUI.OnFullyCharged);
            tank.SubscribeToUltimateExecutedEvent(ultimateUI.OnUltimateExecuted);
        }

        private void ConfigureHealthUIWithTankController(
            HealthUI healthUI,
            TankBrain tank)
        {
            tank.SubscribeToTankInitializedEvent(healthUI.SetTankIcon);
            tank.SubscribeToHealthUpdatedEvent(healthUI.UpdateHealth);
        }

        private void OnTankDead(bool isPlayer)
        {
            if (!isPlayer)
                return;

            m_PlayerController.OnTankDead();

            HelperMethods.InvokeAfterAsync(3, () => Respawn(), m_CTS.Token);
        }
    }
}

