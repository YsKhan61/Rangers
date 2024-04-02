using BTG.Tank;
using BTG.Tank.UltimateAction;

namespace BTG.Enemy
{
    public class EnemyController
    {
        private TankBrain m_TankController;

        public EnemyController(TankBrain tankController)
        {
            m_TankController = tankController;
        }


        public void OnUltimateFullyCharged(IUltimateAction ultimate)
        {
            ultimate.TryExecute();
        }
    }
}
