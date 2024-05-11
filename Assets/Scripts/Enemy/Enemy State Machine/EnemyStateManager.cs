using BTG.Utilities;

namespace BTG.Enemy
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Dead
    }

    public class EnemyStateManager : StateManager<EnemyState>, IUpdatable, IDestroyable
    {
        private EnemyTankController m_Controller;

        public EnemyStateManager(EnemyTankController enemyController)
        {
            m_Controller = enemyController;

            InitializeIdleState();
            InitializeMoveState();
            InitializeAttackState();
            InitializeDeadState();  
        }

        public void Init()
        {
            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);
        }

        public void DeInit()
        {
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }

        public void OnDestroy()
        {
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }

        private void InitializeIdleState()
        {
            EnemyIdleState idleState = new EnemyIdleState(EnemyState.Idle);
            idleState.SetController(m_Controller);
            States.Add(EnemyState.Idle, idleState);
        }

        private void InitializeMoveState()
        {
            EnemyPatrolState moveState = new EnemyPatrolState(EnemyState.Move);
            moveState.SetController(m_Controller);
            States.Add(EnemyState.Move, moveState);
        }

        private void InitializeAttackState()
        {
            EnemyAttackState attackState = new EnemyAttackState(EnemyState.Attack);
            attackState.SetController(m_Controller);
            States.Add(EnemyState.Attack, attackState);
        }

        private void InitializeDeadState()
        {
            EnemyDeadState deadState = new EnemyDeadState(EnemyState.Dead);
            deadState.SetController(m_Controller);
            States.Add(EnemyState.Dead, deadState);
        }
    }
}
