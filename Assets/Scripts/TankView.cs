using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankView : MonoBehaviour
{
    private TankController m_TankController;

    public void SetController(TankController controller)
    {
        m_TankController = controller;
    }
}
