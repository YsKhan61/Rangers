using BTG.Utilities;


namespace BTG.Enemy
{
    /// <summary>
    /// All Enemy States should inherit from this class.
    /// </summary>
    public abstract class EnemyBaseState : BaseState<EnemyStateManager.EnemyState>
    {
        protected EnemyController m_Controller;

        public EnemyBaseState(EnemyStateManager.EnemyState state) : base(state)
        {

        }

        public void SetController(EnemyController controller)
            => m_Controller = controller;

    }
}
