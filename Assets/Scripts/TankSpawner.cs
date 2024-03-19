using UnityEngine;

public class TankSpawner : MonoBehaviour
{
    [SerializeField] TankDataSO m_TankData;

    // Start is called before the first frame update
    void Start()
    {
        CreateTank();
    }

    void CreateTank()
    {
        new TankController(m_TankData);
    }
}
