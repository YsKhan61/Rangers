using BTG.Tank;
using BTG.Tank.UltimateAction;
using BTG.Utilities;
using UnityEngine;
using UnityEngine.AI;


namespace BTG.Enemy
{
    public class EnemyController : IUpdatable
    {
        private EnemyDataSO m_Data;
        private EnemyPool m_Pool;
        private TankBrain m_TankBrain;
        private EnemyView m_View;
        private NavMeshAgent m_Agent;

        private int m_LastIndex = 0;

        public EnemyController(EnemyDataSO data, EnemyPool pool)
        {
            m_Data = data;
            m_View = Object.Instantiate(m_Data.EnemyPrefab, pool.EnemyContainer);
            m_Agent = m_View.GetComponent<NavMeshAgent>();
        }

        ~EnemyController()
        {
            m_TankBrain.OnDeath -= OnDeath;
            UnityCallbacks.Instance.Unregister(this);

            m_TankBrain = null;
        }

        public void Update()
        {
            // UpdateLookDirection();
            
            // check if enemy is near destination, if yes, set new destination
            if (HasReachedDestination())
                SetNewDestination();
        }

        public void Init()
        {
            Debug.Log("Enemy initialized");
            m_TankBrain.SubscribeToFullyChargedEvent(OnUltimateFullyCharged);
            m_TankBrain.OnDeath += OnDeath;

            m_Agent.SetDestination(Vector3.zero);
            m_Agent.acceleration = m_TankBrain.Model.Acceleration;
            m_Agent.stoppingDistance = m_Data.StoppingDistance;

            UnityCallbacks.Instance.Register(this);
        }

        public void SetPose(in Pose pose) => m_View.transform.SetPose(pose);

        public void SetTankBrain(TankBrain tankBrain)
        {
            m_TankBrain = tankBrain;
            m_TankBrain.SetParent(m_View.transform, Vector3.zero, Quaternion.identity);
        }


        public void OnUltimateFullyCharged(IUltimateAction ultimate)
        {
            ultimate.TryExecute();
        }

        private void SetNewDestination()
        {
            int newIndex = m_LastIndex;
            while (newIndex == m_LastIndex)
            {
                newIndex = Random.Range(0, m_Data.PatrolPoints.Length);
            }
            m_LastIndex = newIndex;

            m_Agent.SetDestination(m_Data.PatrolPoints[newIndex]);
        }

        private void UpdateLookDirection()
        {
            // Make sure to look towards destination with a slerp
            Vector3 lookDir = m_Agent.steeringTarget - m_View.transform.position;
            m_View.transform.rotation = Quaternion.Slerp(m_View.transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * m_Data.LookAtSpeed);

        }

        private void OnDeath()
        {
            m_TankBrain.OnDeath -= OnDeath;
            UnityCallbacks.Instance.Unregister(this);

            m_TankBrain = null;
            m_Pool.ReturnEnemy(this);
            Debug.Log("Enemy died!");
        }

        private bool HasReachedDestination()
        {
            return !m_Agent.pathPending 
                && m_Agent.remainingDistance < m_Agent.stoppingDistance
                && (!m_Agent.hasPath || m_Agent.velocity.sqrMagnitude == 0f);
        }
    }
}
