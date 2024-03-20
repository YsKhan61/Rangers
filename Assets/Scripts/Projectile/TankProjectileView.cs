using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectileView : MonoBehaviour
{
    [SerializeField] Rigidbody m_Rigidbody;
    public Rigidbody Rigidbody => m_Rigidbody;

    TankProjectileController m_Controller;


    public void SetController(TankProjectileController controller)
    {
        m_Controller = controller;
    }
}
