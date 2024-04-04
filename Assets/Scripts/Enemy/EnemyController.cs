using BTG.Tank;
using BTG.Tank.UltimateAction;
using BTG.Utilities;
using UnityEngine;
using UnityEngine.AI;


namespace BTG.Enemy
{
    public class EnemyController : IUpdatable
    {
        private EnemyPool m_Pool;
        private TankBrain m_TankBrain;
        private EnemyView m_View;
        private NavMeshAgent m_Agent;

        public EnemyController(EnemyView prefab, EnemyPool pool)
        {
            m_View = Object.Instantiate(prefab, pool.EnemyContainer);
            m_Agent = m_View.GetComponent<NavMeshAgent>();
        }

        ~EnemyController()
        {
            m_TankBrain.OnDeath -= OnDeath;
            // UnityCallbacks.Instance.Unregister(this);

            m_TankBrain = null;
        }

        public void Update()
        {
            m_Agent.Move(m_Agent.transform.forward * Time.deltaTime * m_TankBrain.Model.Acceleration);
        }

        public void Init()
        {
            Debug.Log("Enemy initialized");
            m_TankBrain.SubscribeToFullyChargedEvent(OnUltimateFullyCharged);
            m_TankBrain.OnDeath += OnDeath;

            // UnityCallbacks.Instance.Register(this);
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

        private void OnDeath()
        {
            m_TankBrain.OnDeath -= OnDeath;
            // UnityCallbacks.Instance.Unregister(this);

            m_TankBrain = null;
            m_Pool.ReturnEnemy(this);
            Debug.Log("Enemy died!");
        }
    }
}
