using UnityEngine;
using UnityEngine.AI;

namespace BTG.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyView : MonoBehaviour
    {
        private Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        private EnemyController m_Controller;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        public void SetController(EnemyController controller)
            => m_Controller = controller;

        private void OnDrawGizmos()
        {
            m_Controller?.OnDrawGizmos();
        }
    }
}