using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    public class ProjectileView : MonoBehaviour, IFiringView
    {
        [SerializeField] Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        [SerializeField] private Collider m_Collider;

        public Transform Owner {get; private set;}

    private ProjectileController m_Controller;

        private void OnEnable()
        {
            m_Collider.enabled = true;
        }

        /// <summary>
        /// This is for environment objects
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            m_Controller.OnHitObject(collision.collider);
            m_Collider.enabled = false;
        }

        private void OnDisable()
        {
            m_Collider.enabled = false;
        }

        public void Config(ProjectileController controller)
        {
            m_Controller = controller;
        }

        public void SetOwner(Transform owner)
        {
            Owner = owner;
        }
    }
}

