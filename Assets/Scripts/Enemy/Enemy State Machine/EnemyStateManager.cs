using BTG.Utilities;

namespace BTG.Enemy
{
    public class EnemyStateManager : StateManager<EnemyStateManager.EnemyState>, IUpdatable, IDestroyable
    {
        private EnemyController m_Controller;

        public enum EnemyState
        {
            Idle,
            Move,
            Attack,
            Dead
        }

        public EnemyStateManager(EnemyController enemyController)
        {
            m_Controller = enemyController;

            States.Add(EnemyState.Idle, new EnemyIdleState(EnemyState.Idle));

            InitializeMoveState();

            States.Add(EnemyState.Attack, new EnemyAttackState(EnemyState.Attack));
            States.Add(EnemyState.Dead, new EnemyDeadState(EnemyState.Dead));
            
        }

        public void Init()
        {
            UnityCallbacks.Instance.Register(this as IUpdatable);
            UnityCallbacks.Instance.Register(this as IDestroyable);

            ChangeState(EnemyState.Move);
        }

        public void OnDestroy()
        {
            UnityCallbacks.Instance.Unregister(this as IUpdatable);
            UnityCallbacks.Instance.Unregister(this as IDestroyable);
        }

        private void InitializeMoveState()
        {
            EnemyMoveState moveState = new EnemyMoveState(EnemyState.Move);
            moveState.SetController(m_Controller);
            States.Add(EnemyState.Move, moveState);
        }
    }
}
