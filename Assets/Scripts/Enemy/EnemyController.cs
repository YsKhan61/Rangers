using BTG.Tank;
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
        private NavMeshAgent m_Agent;
        public NavMeshAgent Agent => m_Agent;

        private EnemyStateManager m_StateManager;
        public Rigidbody Rigidbody => m_View.Rigidbody;
        public Transform Transform => m_View.transform;


        public EnemyController(EnemyDataSO data, EnemyPool pool)
        {
            m_Pool = pool;
            m_Data = data;
            m_View = Object.Instantiate(m_Data.EnemyPrefab, pool.EnemyContainer);
            m_View.SetController(this);
            m_Agent = m_View.GetComponent<NavMeshAgent>();
            m_StateManager = new EnemyStateManager(this);

            Rigidbody.maxLinearVelocity = m_Data.MaxSpeedMultiplier * m_Data.MaxSpeedMultiplier;
        }

        ~EnemyController()
        {
            m_TankBrain.OnAfterDeath -= OnTankDeath;

            m_TankBrain = null;
        }

        public void Init()
        {
            m_Agent.speed = m_TankBrain.Model.MaxSpeed * m_Data.MaxSpeedMultiplier;
            m_Agent.stoppingDistance = m_Data.StoppingDistance;

            m_StateManager.Init();
        }

        public void SetPose(in Pose pose) => m_View.transform.SetPose(pose);

        public void SetTank(
            TankBrain tank,
            int selfLayer, 
            int oppositionLayer)
        {
            m_TankBrain = tank;

            m_TankBrain.Model.IsPlayer = false;
            m_TankBrain.SetLayers(selfLayer, oppositionLayer);
            m_TankBrain.SetParentOfView(m_View.transform, Vector3.zero, Quaternion.identity);
            m_TankBrain.SetRigidbody(Rigidbody);
            m_TankBrain.SubscribeToFullyChargedEvent(OnUltimateFullyCharged);
            m_TankBrain.OnAfterDeath += OnTankDeath;
            m_TankBrain.Init();
        }


        public void OnUltimateFullyCharged()
        {
            m_TankBrain.TryExecuteUltimate();
        }


        private void OnTankDeath()
        {
            m_TankBrain.OnAfterDeath -= OnTankDeath;

            m_StateManager.ChangeState(EnemyStateManager.EnemyState.Dead);
            m_StateManager.DeInit();

            m_TankBrain = null;
            m_Pool.ReturnEnemy(this);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Transform.position, m_Agent.destination);
        }
    }
}
