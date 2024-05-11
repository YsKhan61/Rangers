using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        [SerializeField] private Collider m_Collider;

        private ProjectileController m_Controller;

        private void OnEnable()
        {
            m_Collider.enabled = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            m_Controller.OnHitObject(collision.collider);
            m_Collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            m_Controller.OnHitObject(other);
            m_Collider.enabled = false;
        }

        private void OnDisable()
        {
            m_Collider.enabled = false;
        }

        public void SetController(ProjectileController controller)
        {
            m_Controller = controller;
        }
    }
}

