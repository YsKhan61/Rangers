using UnityEngine;
using UnityEngine.AI;

namespace BTG.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private HealthUIView m_HealthUIView;

        private Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        private EnemyTankController m_Controller;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        public void SetController(EnemyTankController controller)
            => m_Controller = controller;

        /// <summary>
        /// Updates the health UI with the given value.
        /// </summary>
        public void UpdateHealthUI(int currentHealth, int maxHealth)
            => m_HealthUIView.UpdateHealthUI((float)currentHealth/maxHealth);

        private void OnDrawGizmos()
        {
            m_Controller?.OnDrawGizmos();
        }
    }
}