using UnityEngine;

public class TankProjectileView : MonoBehaviour
{
    [SerializeField] Rigidbody m_Rigidbody;
    public Rigidbody Rigidbody => m_Rigidbody;

    [SerializeField] ParticleSystem m_TrailParticles;

    TankProjectileController m_Controller;


    public void SetController(TankProjectileController controller)
    {
        m_Controller = controller;
    }
}
