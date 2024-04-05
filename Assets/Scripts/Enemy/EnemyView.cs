using UnityEngine;
using UnityEngine.AI;

namespace BTG.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyView : MonoBehaviour
    {
        private EnemyController m_Controller;

        public void SetController(EnemyController controller)
            => m_Controller = controller;

        private void OnDrawGizmos()
        {
            m_Controller?.OnDrawGizmos();
        }
    }
}