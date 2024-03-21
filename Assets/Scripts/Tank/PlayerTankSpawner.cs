using Cinemachine;
using UnityEngine;

public class PlayerTankSpawner : MonoBehaviour
{
    [SerializeField] TankDataContainerSO m_TankDataList;
    [SerializeField] PlayerVirualCameraController m_PVCController;

    // Start is called before the first frame update
    void Start()
    {
        if (TryCreatePlayerTank(out TankController controller))
        {
            ConfigurePlayerCameraWithController(controller);
        }
    }

    private bool TryCreatePlayerTank(out TankController controller)
    {
        controller = null;

        if (!TryGetTankById(PlayerPrefs.GetInt("TankID", 1), out TankDataSO tankDataToSpawn))
            return false;

        controller = new TankController(tankDataToSpawn);
        return true;
    }
        

    private void ConfigurePlayerCameraWithController(in TankController controller)
    {
        m_PVCController.SetFollow(controller.CameraTarget);
        controller.TankFiring.OnTankShoot += m_PVCController.ShakeCamera;
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
