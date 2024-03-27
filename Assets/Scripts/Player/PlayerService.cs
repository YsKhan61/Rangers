using BTG.Tank;
using BTG.UI;


namespace BTG.Player
{
    // public class PlayerService : MonoBehaviour
    public class PlayerService
    {
        private PlayerInputs m_PlayerInputs;

        public void ConfigurePlayerTank(
            in int tankId,
            in TankFactory tankFactory,
            in PlayerVirualCameraController pvc, 
            in UltimateUI ultimateUI)
        {
            if (!tankFactory.TryGetTank(tankId, out TankController controller))
            {
                return;
            }

            ConfigurePlayerCameraWithController(pvc, controller);
            ConfigureUltimateUIWithController(ultimateUI, controller);

            m_PlayerInputs = new PlayerInputs(controller);
            m_PlayerInputs.Start();
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


        private void ConfigurePlayerCameraWithController(
            in PlayerVirualCameraController pvc,
            in TankController controller)
        {
            pvc.Initialize(controller.CameraTarget);
            controller.SubscribeToOnTankShootEvent(pvc.ShakeCameraOnPlayerTankShoot);
            controller.SubscribeToCameraShakeEvent(pvc.ShakeCameraOnUltimateExecution);
        }

        private void ConfigureUltimateUIWithController(
            in UltimateUI ultimateUI,
            in TankController controller)
        {
            controller.SubscribeToUltimateActionAssignedEvent(ultimateUI.AssignUltimateActionName);
            controller.SubscribeToChargeUpdatedEvent(ultimateUI.UpdateChargeAmount);
            controller.SubscribeToFullyChargedEvent(ultimateUI.OnFullyCharged);
            controller.SubscribeToUltimateExecutedEvent(ultimateUI.OnUltimateExecuted);
        }
    }
}

