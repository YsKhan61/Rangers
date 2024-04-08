using BTG.Tank;
using BTG.Tank.UltimateAction;
using BTG.Utilities;
using UnityEngine;
using UnityEngine.AI;


namespace BTG.Enemy
{
    public class EnemyController
    {
        private EnemyDataSO m_Data;
        public EnemyDataSO Data => m_Data;

        private EnemyPool m_Pool;
        private TankBrain m_TankBrain;
        private EnemyView m_View;
        private Rigidbody m_Rigidbody;
        private NavMeshAgent m_Agent;
        public NavMeshAgent Agent => m_Agent;

        private Transform m_Transform;

        private EnemyStateManager m_StateManager;


        public EnemyController(EnemyDataSO data, EnemyPool pool)
        {
            m_Data = data;

            m_View = Object.Instantiate(m_Data.EnemyPrefab, pool.EnemyContainer);
            m_View.SetController(this);

            m_Rigidbody = m_View.GetComponent<Rigidbody>();
            m_Rigidbody.maxLinearVelocity = m_Data.MaxSpeedMultiplier * m_Data.MaxSpeedMultiplier;

            m_Agent = m_View.GetComponent<NavMeshAgent>();

            m_Transform = m_View.transform;

            m_StateManager = new EnemyStateManager(this);
        }

        ~EnemyController()
        {
            m_TankBrain.OnAfterDeath -= OnDeath;

            m_TankBrain = null;
        }

        public void Init()
        {
            m_Agent.speed = m_TankBrain.Model.MaxSpeed * m_Data.MaxSpeedMultiplier;
            m_Agent.stoppingDistance = m_Data.StoppingDistance;

            m_StateManager.Init();
        }

        public void SetPose(in Pose pose) => m_View.transform.SetPose(pose);

        public void SetTankBrain(TankBrain tankBrain)
        {
            m_TankBrain = tankBrain;
            m_TankBrain.SetParentOfView(m_View.transform, Vector3.zero, Quaternion.identity);
            m_TankBrain.SetRigidbody(m_Rigidbody);
            m_TankBrain.SubscribeToFullyChargedEvent(OnUltimateFullyCharged);
            m_TankBrain.OnAfterDeath += OnDeath;
        }


        public void OnUltimateFullyCharged(IUltimateAction ultimate)
        {
            ultimate.TryExecute();
        }


        private void OnDeath()
        {
            m_TankBrain.OnAfterDeath -= OnDeath;

            m_StateManager.ChangeState(EnemyStateManager.EnemyState.Dead);

            m_TankBrain = null;
            m_Pool.ReturnEnemy(this);
            Debug.Log("Enemy died!");
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(m_Transform.position, m_Agent.destination);
        }
    }
}
