using BTG.EventSystem;
using BTG.Tank;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace BTG.Enemy
{
    public class EnemyService
    {
        private TankFactory m_TankFactory;
        private WaveConfigSO m_EnemyWaves;
        private CancellationTokenSource m_Cts;

        public EnemyService(TankFactory tankFactory, WaveConfigSO enemyWaves)
        {
            m_Cts = new CancellationTokenSource();
            m_TankFactory = tankFactory;
            EventService.Instance.OnTankDead.AddListener(OnTankDead);
            m_EnemyWaves = enemyWaves;
        }

        ~EnemyService()
        {
            m_TankFactory = null;
            EventService.Instance.OnTankDead.RemoveListener(OnTankDead);

            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        public void StartNextWave()
        {
            if (!m_EnemyWaves.GetTanksForNextWave(out int[] tankIDs))
            {
                Debug.Log("No more waves");
                return;
            }

            if (tankIDs == null)
            {
                Debug.Log("No more waves");
                return;
            }

            foreach (int tankId in tankIDs)
            {
                SpawnEnemyTank(tankId);
            }
        }

        public void SpawnEnemyTank(int tankId)
        {
            if (!m_TankFactory.TryGetTank(tankId, out TankMainController controller))
            {
                if (!m_TankFactory.TryGetRandomTank(out controller))
                {
                    Debug.Log("Failed to spawn enemy tank");
                    return;
                }
            }
            
            Pose pose = m_EnemyWaves.GetRandomSpawnPose();
            controller.Transform.position = pose.position;
            controller.Transform.rotation = pose.rotation;

            controller.Model.IsPlayer = false;
        }

        private void OnTankDead(bool isPlayer)
        {
            if (isPlayer)
            {
                // Game Over logics
            }
            else
            {
                OnEnemyTankDead();
            }
        }

        private void OnEnemyTankDead() 
        {
            m_EnemyWaves.TankCountInCurrentWave--;
            if (m_EnemyWaves.TankCountInCurrentWave > 0)
            {
                return;
            }

            _ = InvokeAsync(m_EnemyWaves.Interval, () =>
            {
                StartNextWave();
            });
        }

        private async Task InvokeAsync(float seconds, Action actionAfterWait)
        {
            await Task.Delay((int)(seconds * 1000), m_Cts.Token);
            actionAfterWait();
        }
    }
}
