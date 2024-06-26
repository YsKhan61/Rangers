using UnityEngine;


namespace BTG.Actions.PrimaryAction
{


    public class ProjectileView : MonoBehaviour, IProjectileView
    {
        [SerializeField] 
        Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        [SerializeField]
        Collider m_Collider;

        public Transform Transform => transform;
        public Transform Owner {get; private set;}

        private ProjectileController m_Controller;
        private ProjectilePool m_Pool;


        /// <summary>
        /// This is for environment objects
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            m_Controller.OnHitSomething(collision.collider);
            InvokeEffectEvents();
        }

        private void OnTriggerEnter(Collider other)
        {
            m_Controller.OnHitSomething(other);
            InvokeEffectEvents();
        }

        public void SetPool(ProjectilePool pool) => m_Pool = pool;
        public void SetController(ProjectileController controller) => m_Controller = controller;
        public void SetOwner(Transform owner) => Owner = owner;
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation) => transform.SetPositionAndRotation(position, rotation);

        public void Show() => gameObject.SetActive(true);
        
        public void Hide() => gameObject.SetActive(false);
        public void ReturnToPool() => m_Pool.ReturnProjectile(this);

        private void InvokeEffectEvents()
        {
            // Invoke the effect events such as explosion effect and audio
        }
    }
}

