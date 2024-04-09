using BTG.EventSystem;
using BTG.Tank;
using BTG.Utilities;
using BTG.Utilities.DI;
using System.Threading;
using UnityEngine;


namespace BTG.Enemy
{
    public class EnemyService
    {
        private TankFactory m_TankFactory;
        private WaveConfigSO m_EnemyWaves;
        private CancellationTokenSource m_Cts;

        private int m_NextWaveIndex = 0;
        private int m_TankCountInCurrentWave = 0;

        /*private int m_PlayerLayer;
        private int m_EnemyLayer;*/

        private EnemyPool m_EnemyPool;

        [Inject]
        private IntDataSO m_EnemyDeathCountData;

        public EnemyService(
            TankFactory tankFactory, 
            WaveConfigSO enemyWaves,
            /*int playerLayer,
            int enemyLayer,*/
            EnemyDataSO enemyData)
        {
            m_Cts = new CancellationTokenSource();
            m_TankFactory = tankFactory;
            m_EnemyWaves = enemyWaves;

            /*m_PlayerLayer = playerLayer;
            m_EnemyLayer = enemyLayer;*/
            m_NextWaveIndex = 0;

            m_EnemyPool = new EnemyPool(enemyData);
        }

        ~EnemyService()
        {
            m_TankFactory = null;

            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        public void OnEnemyDeath()
        {
            m_EnemyDeathCountData.Value++;
            TryStartNextWave();
        }

        public void StartNextWave()
        {
            if (!m_EnemyWaves.GetTanksForNextWave(m_NextWaveIndex, out int[] tankIDs))
            {
                Debug.Log("No tanks found!");
                return;
            }

            m_TankCountInCurrentWave = tankIDs.Length;

            foreach (int tankId in tankIDs)
            {
                ConfigureTankAndController(tankId);
            }
        }

        private void ConfigureTankAndController(int tankId)
        {
            bool tankFound = GetEnemyTank(tankId, out TankBrain tank);
            if (!tankFound)
                return;

            bool found = GetEnemyController(out EnemyController controller);
            if (!found)
                return;

            // controller.SetTank(tank, m_EnemyLayer, m_PlayerLayer);
            controller.SetTank(tank);
            controller.SetService(this);
            Pose pose = m_EnemyWaves.GetRandomSpawnPose();
            controller.SetPose(pose);
            controller.Init();
        }

        private bool GetEnemyTank(int tankId, out TankBrain tank)
        {
            if (!m_TankFactory.TryGetTank(tankId, out tank))
            {
                if (!m_TankFactory.TryGetRandomTank(out tank))
                {
                    Debug.Log("Failed to spawn enemy tank");
                    return false;
                }
            }
            return true;
        }

        private bool GetEnemyController(out EnemyController controller)
        {
            controller = m_EnemyPool.GetEnemy();
            if (controller == null)
            {
                Debug.Log("Failed to spawn enemy controller");
                return false;
            }
            
            return true;
        }

        private void TryStartNextWave()
        {
            m_TankCountInCurrentWave--;
            if (m_TankCountInCurrentWave > 0)
            {
                return;
            }

            m_NextWaveIndex++;

            _ = HelperMethods.InvokeAfterAsync(m_EnemyWaves.Interval, () =>
            {
                StartNextWave();
            }, m_Cts.Token);
        }
    }
}
