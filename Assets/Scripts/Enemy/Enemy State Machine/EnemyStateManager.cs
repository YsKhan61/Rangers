using BTG.Utilities;

namespace BTG.Enemy
{
    public class EnemyStateManager : StateManager<EnemyStateManager.EnemyState>, IUpdatable, IDestroyable
    {
        private EnemyTankController m_Controller;

        public enum EnemyState
        {
            Idle,
            Move,
            Attack,
            Dead
        }

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
            UnityCallbacks.Instance.RegisterToUpdatable(this as IUpdatable);
            UnityCallbacks.Instance.RegisterToDestroyable(this as IDestroyable);

            ChangeState(EnemyState.Move);
        }

        public void DeInit()
        {
            UnityCallbacks.Instance.UnregisterFromUpdatable(this as IUpdatable);
            UnityCallbacks.Instance.UnregisterFromDestroy(this as IDestroyable);
        }

        public void OnDestroy()
        {
            UnityCallbacks.Instance.UnregisterFromUpdatable(this as IUpdatable);
            UnityCallbacks.Instance.UnregisterFromDestroy(this as IDestroyable);
        }

        private void InitializeIdleState()
        {
            EnemyIdleState idleState = new EnemyIdleState(EnemyState.Idle);
            idleState.SetController(m_Controller);
            States.Add(EnemyState.Idle, idleState);
        }

        private void InitializeMoveState()
        {
            EnemyMoveState moveState = new EnemyMoveState(EnemyState.Move);
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
