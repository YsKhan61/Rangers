using BTG.EventSystem;
using BTG.Tank;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace BTG.Enemy
{
    public class EnemyService
    {
        private TankFactory m_TankFactory;

        private CancellationTokenSource m_Cts;

        public EnemyService(TankFactory tankFactory)
        {
            m_Cts = new CancellationTokenSource();
            m_TankFactory = tankFactory;
            EventService.Instance.OnTankDead.AddListener(OnTankDead);
        }

        ~EnemyService()
        {
            m_TankFactory = null;
            EventService.Instance.OnTankDead.RemoveListener(OnTankDead);

            m_Cts.Cancel();
            m_Cts.Dispose();
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

            _ = InvokeAsync(3, () =>
            {
                SpawnEnemyTank(0);
            });
        }

        private async Task InvokeAsync(float seconds, Action actionAfterWait)
        {
            await Task.Delay((int)(seconds * 1000), m_Cts.Token);
            actionAfterWait();
        }
    }
}
