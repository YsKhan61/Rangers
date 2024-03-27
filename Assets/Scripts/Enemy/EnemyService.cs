using BTG.Tank;
using System.Numerics;


namespace BTG.Enemy
{
    public class EnemyService
    {
        private TankFactory m_TankFactory;

        public EnemyService(TankFactory tankFactory)
        {
            m_TankFactory = tankFactory;
        }

        public void SpawnEnemyTank(
            in int tankId)
        {
            m_TankFactory.TryCreatePlayerTank(in tankId, out TankController controller);
            controller.Transform.position = new UnityEngine.Vector3(-5, 0, 0);
        }
    }
}
