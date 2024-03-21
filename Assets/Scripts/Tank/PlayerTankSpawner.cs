using Cinemachine;
using UnityEngine;

public class PlayerTankSpawner : MonoBehaviour
{
    [SerializeField] TankDataContainerSO m_TankDataList;
    [SerializeField] CinemachineVirtualCamera m_PVC1;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayerTank();
    }

    void CreatePlayerTank()
    {
        if (!TryGetTankById(PlayerPrefs.GetInt("TankID", 1), out TankDataSO tankDataToSpawn))
            return;

        TankController controller = new TankController(tankDataToSpawn);
        m_PVC1.Follow = controller.CameraTarget;
    }

    bool TryGetTankById(in int id, out TankDataSO tankData)
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
