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
            if (other.TryGetComponent(out IDamageableView view))
            {
                if (view.IsPlayer)
                {
                    m_Controller.UpdatePlayerView(view);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IDamageableView view))
            {
                m_Controller.UpdatePlayerView(null);
            }
        }

        public void SetController(EnemyTankController controller)
            => m_Controller = controller;

        /// <summary>
        /// Updates the health UI with the given value.
        /// </summary>
        public void UpdateHealthUI(int currentHealth, int maxHealth)
            => m_HealthUIView.UpdateHealthUI((float)currentHealth/maxHealth);

        /// <summary>
        /// Toggle the visibility of the health UI.
        /// </summary>
        public void ToggleUIVisibility(bool isVisible)
        {
            m_HealthUIView.ToggleVisibility(isVisible);
        }

        /// <summary>
        /// Show or Hide this view
        /// </summary>
        public void ToggleView(bool value) => gameObject.SetActive(value);


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