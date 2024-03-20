using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectileView : MonoBehaviour
{
    TankProjectileController m_Controller;

    private void Update()
    {
        m_Controller.Update();
    }

    public void SetController(TankProjectileController controller)
    {
        m_Controller = controller;
    }
}
