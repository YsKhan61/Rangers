using Cinemachine;
using UnityEngine;

public class PlayerTankSpawner : MonoBehaviour
{
    [SerializeField] TankDataSO m_TankData;
    [SerializeField] CinemachineVirtualCamera m_PVC1;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayerTank();
    }

    void CreatePlayerTank()
    {
        TankController controller = new TankController(m_TankData);
        m_PVC1.Follow = controller.Transform;
    }
}
