using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject m_TankPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(m_TankPrefab, transform.position, transform.rotation);
    }
}
