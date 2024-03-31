using BTG.EventSystem;
using BTG.Tank;
using BTG.UI;
using BTG.Utilities;
using System.Threading;


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
        private PlayerVirualCameraController m_PVC;    // temporary for now

        private CancellationTokenSource m_CTS;

        public void Initialize(
            in int tankID,
            in TankFactory tankFactory,
            in PlayerVirualCameraController pvc, 
            in UltimateUI ultimateUI,
            int playerLayer,
            int enemyLayer)
        {
            m_TankID = tankID;
            m_TankFactory = tankFactory;
            m_PlayerLayer = playerLayer;
            m_EnemyLayer = enemyLayer;
            m_PVC = pvc;
            m_UltimateUI = ultimateUI;

            m_PlayerController = new PlayerController();
            InitializePlayerInput(m_PlayerController);

            CreateAndConfigureTankWithPlayer();

            EventService.Instance.OnTankDead.AddListener(OnTankDead);

            m_CTS = new CancellationTokenSource();
        }

        ~PlayerService()
        {
            EventService.Instance.OnTankDead.RemoveListener(OnTankDead);
            m_CTS.Cancel();
            m_CTS.Dispose();
        }

        private void CreateAndConfigureTankWithPlayer()
        {
            CreatePlayerTank(out TankMainController tank);

            m_PlayerController.SetTank(tank);
            ConfigurePlayerCameraWithTankController(m_PVC, tank);
            ConfigureUltimateUIWithTankController(m_UltimateUI, tank);
        }

        private void CreatePlayerTank(out TankMainController controller)
        {
            if (!m_TankFactory.TryGetTank(m_TankID, out controller))
            {
                return;
            }

            controller.Transform.position = new UnityEngine.Vector3(0, 0, 0);
            controller.Transform.rotation = UnityEngine.Quaternion.identity;
            controller.ToggleTankVisibility(true);
            controller.Model.IsPlayer = true;
            controller.SetLayers(m_PlayerLayer, m_EnemyLayer);
        }

        private void InitializePlayerInput(PlayerController controller)
        {
            new PlayerInputs(controller).Initialize();
        }

        private void ConfigurePlayerCameraWithTankController(
            in PlayerVirualCameraController pvc,
            in TankMainController controller)
        {
            pvc.Initialize(controller.CameraTarget);
            controller.SubscribeToOnTankShootEvent(pvc.ShakeCameraOnPlayerTankShoot);
            controller.SubscribeToCameraShakeEvent(pvc.ShakeCameraOnUltimateExecution);
        }

        private void ConfigureUltimateUIWithTankController(
            in UltimateUI ultimateUI,
            in TankMainController controller)
        {
            controller.SubscribeToUltimateActionAssignedEvent(ultimateUI.AssignUltimateActionName);
            controller.SubscribeToChargeUpdatedEvent(ultimateUI.UpdateChargeAmount);
            controller.SubscribeToFullyChargedEvent(ultimateUI.OnFullyCharged);
            controller.SubscribeToUltimateExecutedEvent(ultimateUI.OnUltimateExecuted);
        }

        private void OnTankDead(bool isPlayer)
        {
            if (!isPlayer)
                return;

            m_PlayerController.OnTankDead();

            HelperMethods.InvokeAfterAsync(3, () => CreateAndConfigureTankWithPlayer(), m_CTS.Token);
        }
    }
}

