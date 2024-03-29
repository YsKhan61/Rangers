using BTG.Tank;
using BTG.Tank.UltimateAction;

namespace BTG.Enemy
{
    public class EnemyController
    {
        private TankMainController m_TankController;

        public EnemyController(TankMainController tankController)
        {
            m_TankController = tankController;
        }


        public void OnUltimateFullyCharged(IUltimateAction ultimate)
        {
            ultimate.TryExecute();
        }
    }
}
