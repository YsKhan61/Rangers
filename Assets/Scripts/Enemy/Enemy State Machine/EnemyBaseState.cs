using BTG.Utilities;


namespace BTG.Enemy
{
    /// <summary>
    /// All Enemy States should inherit from this class.
    /// </summary>
    public abstract class EnemyBaseState : BaseState<EnemyState>
    {
        protected EnemyTankController m_Controller;

        public EnemyBaseState(EnemyState state) : base(state)
        {

        }

        public void SetController(EnemyTankController controller)
            => m_Controller = controller;

    }
}
