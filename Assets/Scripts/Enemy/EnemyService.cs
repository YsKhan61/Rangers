using BTG.EventSystem;
using BTG.Tank;
using System.Diagnostics;


namespace BTG.Enemy
{
    public class EnemyService
    {
        private TankFactory m_TankFactory;

        public EnemyService(TankFactory tankFactory)
        {
            m_TankFactory = tankFactory;
            EventService.Instance.OnTankDead.AddListener(OnTankDead);
        }

        ~EnemyService()
        {
            m_TankFactory = null;
            EventService.Instance.OnTankDead.RemoveListener(OnTankDead);
        }

        public void SpawnEnemyTank(
            int tankId)
        {
            if (!m_TankFactory.TryGetTank(tankId, out TankMainController controller))
            {
                if (!m_TankFactory.TryGetRandomTank(out controller))
                {
                    Debug.WriteLine("Failed to spawn enemy tank");
                    return;
                }
            }
            
            controller.Transform.position = new UnityEngine.Vector3(-5, 0, 0);
            controller.Model.IsPlayer = false;
        }

        private void OnTankDead(bool isPlayer)
        {
            if (isPlayer) return;
            SpawnEnemyTank(0);
        }
    }
}
