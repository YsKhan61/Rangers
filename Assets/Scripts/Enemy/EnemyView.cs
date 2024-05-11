using BTG.Utilities;
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPlayerView _))
            {
                m_Controller.SetPlayerInRange(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IPlayerView _))
            {
                m_Controller.SetPlayerInRange(false);
            }
        }

        public void SetController(EnemyTankController controller)
            => m_Controller = controller;

        /// <summary>
        /// Updates the health UI with the given value.
        /// </summary>
        public void UpdateHealthUI(int currentHealth, int maxHealth)
            => m_HealthUIView.UpdateHealthUI((float)currentHealth/maxHealth);

        public void ToggleVisibility(bool isVisible)
        {
            m_HealthUIView.ToggleVisibility(isVisible);
        }

        private void OnDrawGizmos()
        {
            m_Controller?.OnDrawGizmos();
        }
    }
}