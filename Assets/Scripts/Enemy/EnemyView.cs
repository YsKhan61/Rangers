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
            if (other.TryGetComponent(out IPlayerView view))
            {
                m_Controller.SetPlayerView(view);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IPlayerView _))
            {
                m_Controller.SetPlayerView(null);
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


#if UNITY_EDITOR
        [Header("Debug purpose")]
        [SerializeField]
        [TextArea(2, 2)]
        private string m_Description;

        /// <summary>
        /// This method is used to log the description of the view in it's inspector
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="append">if true, the message will be added as new line, or will replace the old message</param>
        public void LogDescription(string message)
        {
            m_Description = message;
        }

        private void OnDrawGizmos()
        {
            m_Controller?.OnDrawGizmos();
        }
#endif
    }
}