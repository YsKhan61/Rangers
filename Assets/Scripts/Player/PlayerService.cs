using BTG.Tank;
using BTG.UI;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerService
    {
        private PlayerInputs m_PlayerInputs;

        public void SpawnPlayerTank(
            in int tankId,
            in TankFactory tankFactory,
            in PlayerVirualCameraController pvc, 
            in UltimateUI ultimateUI,
            int playerLayer,
            int enemyLayer)
        {
            if (!tankFactory.TryGetTank(tankId, out TankMainController controller))
            {
                return;
            }

            controller.Model.IsPlayer = true;
            controller.SetLayers(playerLayer, enemyLayer);
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
            in TankMainController controller)
        {
            pvc.Initialize(controller.CameraTarget);
            controller.SubscribeToOnTankShootEvent(pvc.ShakeCameraOnPlayerTankShoot);
            controller.SubscribeToCameraShakeEvent(pvc.ShakeCameraOnUltimateExecution);
        }

        private void ConfigureUltimateUIWithController(
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

