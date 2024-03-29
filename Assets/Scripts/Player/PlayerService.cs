using BTG.Tank;
using BTG.UI;


namespace BTG.Player
{
    public class PlayerService
    {
        private PlayerInputs m_PlayerInputs;

        public void Initialize(
            in int tankId,
            in TankFactory tankFactory,
            in PlayerVirualCameraController pvc, 
            in UltimateUI ultimateUI,
            int playerLayer,
            int enemyLayer)
        {
            CreatePlayer(out PlayerController playerController);
            CreatePlayerTank(tankId, tankFactory, playerLayer, enemyLayer, out TankMainController tankController);
            playerController.SetTank(tankController);
            ConfigurePlayerCameraWithTankController(pvc, tankController);
            ConfigureUltimateUIWithTankController(ultimateUI, tankController);
            InitializePlayerInput(playerController);
        }

        public void Update()
        {
            m_PlayerInputs?.Update();
        }

        public void OnDestroy()
        {
            m_PlayerInputs?.OnDestroy();
            m_PlayerInputs = null;
        }

        private void CreatePlayer(out PlayerController controller)
        {
            controller = new PlayerController();
        }

        private void CreatePlayerTank(
            int tankId, 
            TankFactory tankFactory, 
            int playerLayer, 
            int enemyLayer,
            out TankMainController controller)
        {
            if (!tankFactory.TryGetTank(tankId, out controller))
            {
                return;
            }

            controller.Model.IsPlayer = true;
            controller.SetLayers(playerLayer, enemyLayer);
        }

        private void InitializePlayerInput(PlayerController controller)
        {
            m_PlayerInputs = new PlayerInputs(controller);
            m_PlayerInputs.Start();
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
    }
}

