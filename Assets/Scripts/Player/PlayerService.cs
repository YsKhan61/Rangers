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
        private PlayerVirtualCamera m_PVC;    // temporary for now

        private CancellationTokenSource m_CTS;

        public PlayerService(
            in int tankID,
            in TankFactory tankFactory,
            in PlayerVirtualCamera pvc,
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
        }

        public void Initialize()
        {
            CreatePlayerControllerAndInput();
            Respawn();
            EventService.Instance.OnTankDead.AddListener(OnTankDead);
            m_CTS = new CancellationTokenSource();
        }

        ~PlayerService()
        {
            EventService.Instance.OnTankDead.RemoveListener(OnTankDead);
            m_CTS.Cancel();
            m_CTS.Dispose();
        }

        private void CreatePlayerControllerAndInput()
        {
            m_PlayerController = new PlayerController();
            PlayerInputs playerInput = new PlayerInputs(m_PlayerController);
            playerInput.Initialize();
        }

        private void Respawn()
        {
            CreateAndSpawnPlayerTank(out TankBrain tank);
            ConfigureTankWithPlayer(tank);
        }

        private void CreateAndSpawnPlayerTank(out TankBrain controller)
        {
            if (!m_TankFactory.TryGetTank(m_TankID, out controller))
            {
                return;
            }

            controller.Transform.position = new UnityEngine.Vector3(0, 0, 0);
            controller.Transform.rotation = UnityEngine.Quaternion.identity;
            controller.Model.IsPlayer = true;
            controller.SetLayers(m_PlayerLayer, m_EnemyLayer);
        }

        private void ConfigureTankWithPlayer(TankBrain tank)
        {
            m_PlayerController.SetTank(tank);
            ConfigurePlayerCameraWithTankController(m_PVC, tank);
            ConfigureUltimateUIWithTankController(m_UltimateUI, tank);
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

        private void OnTankDead(bool isPlayer)
        {
            if (!isPlayer)
                return;

            m_PlayerController.OnTankDead();

            HelperMethods.InvokeAfterAsync(3, () => Respawn(), m_CTS.Token);
        }
    }
}

