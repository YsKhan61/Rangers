using UnityEngine;


namespace BTG.Tank.Projectile
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        [SerializeField] ParticleSystem m_TrailParticles;

        ProjectileController m_Controller;


        public void SetController(ProjectileController controller)
        {
            m_Controller = controller;
        }
    }
}

