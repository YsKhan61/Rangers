using BTG.Tank;
using BTG.UI;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerService : MonoBehaviour
    {
        [SerializeField] private int m_TestTankID; // This is for test purpose, so that we dont have to go to MainMenu and then select the tank

        [SerializeField] TankDataContainerSO m_TankDataList;
        [SerializeField] PlayerVirualCameraController m_PVCController;

        [SerializeField] private UltimateUI m_UltimatePanel;

        private PlayerInputs m_PlayerInputs;

        // Start is called before the first frame update
        void Start()
        {
            if (!TryCreatePlayerTank(out TankController controller))
            {
                enabled = false;
                return;
            }

            ConfigurePlayerCameraWithController(controller);
            ConfigureUltimateUIWithController(controller);
            m_PlayerInputs = new PlayerInputs(controller);
            m_PlayerInputs.Start();
        }

        private void Update()
        {
            m_PlayerInputs.Update();
        }

        private void OnDestroy()
        {
            m_PlayerInputs.OnDestroy();
            m_PlayerInputs = null;
        }

        private bool TryCreatePlayerTank(out TankController controller)
        {
            controller = null;

            if (!TryGetTankById(PlayerPrefs.GetInt("TankID", m_TestTankID), out TankDataSO tankDataToSpawn))            // m_TankID is for test purpose
                return false;

            controller = new TankController(tankDataToSpawn);
            return true;
        }


        private void ConfigurePlayerCameraWithController(in TankController controller)
        {
            m_PVCController.Initialize(controller.CameraTarget);
            controller.SubscribeToOnTankShootEvent(m_PVCController.ShakeCameraOnPlayerTankShoot);
            controller.SubscribeToUltimateExecutedEvent(m_PVCController.ShakeCameraOnUltimateExecution);
        }

        private void ConfigureUltimateUIWithController(in TankController controller)
        {
            controller.SubscribeToUltimateActionAssignedEvent(m_UltimatePanel.AssignUltimateActionName);
            controller.SubscribeToChargeUpdatedEvent(m_UltimatePanel.UpdateChargeAmount);
            controller.SubscribeToFullyChargedEvent(m_UltimatePanel.OnFullyCharged);
            controller.SubscribeToUltimateExecutedEvent(m_UltimatePanel.OnUltimateExecuted);
        }

        private bool TryGetTankById(in int id, out TankDataSO tankData)
        {
            tankData = null;

            if (m_TankDataList == null || m_TankDataList.TankDataList.Length == 0)
            {
                Debug.LogError("TankDataList is not set or empty in PlayerTankSpawner");
                return false;
            }

            foreach (var tank in m_TankDataList.TankDataList)
            {
                if (tank.ID == id)
                {
                    tankData = tank;
                    return true;
                }
            }
            return false;
        }
    }
}

