using BTG.Utilities;


namespace BTG.Enemy
{
    public abstract class EnemyBaseState : BaseState<EnemyStateManager.EnemyState>
    {
        protected EnemyController m_Controller;

        public EnemyBaseState(EnemyStateManager.EnemyState state) : base(state)
        {

        }

        public void SetController(EnemyController controller)
            => m_Controller = controller;


        public override void Enter()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
