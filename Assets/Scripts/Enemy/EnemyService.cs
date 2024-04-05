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

        private int m_NextWaveIndex = 0;
        private int m_TankCountInCurrentWave = 0;

        private int m_PlayerLayer;
        private int m_EnemyLayer;

        private EnemyPool m_EnemyPool;

        public EnemyService(
            TankFactory tankFactory, 
            WaveConfigSO enemyWaves,
            int playerLayer,
            int enemyLayer)
        {
            m_Cts = new CancellationTokenSource();
            m_TankFactory = tankFactory;
            EventService.Instance.OnBeforeTankDead.AddListener(OnTankAboutToDie);
            m_EnemyWaves = enemyWaves;

            m_PlayerLayer = playerLayer;
            m_EnemyLayer = enemyLayer;
            m_NextWaveIndex = 0;
        }

        ~EnemyService()
        {
            m_TankFactory = null;
            EventService.Instance.OnBeforeTankDead.RemoveListener(OnTankAboutToDie);

            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        public void CreateEnemyPool(EnemyDataSO data)
        {
            m_EnemyPool = new EnemyPool(data);
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

            tank.Model.IsPlayer = false;
            tank.SetLayers(m_EnemyLayer, m_PlayerLayer);
            tank.Init();

            bool found = GetEnemyController(out EnemyController controller);
            if (!found)
                return;

            controller.SetTankBrain(tank);
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

        /*public void SpawnEnemyTank(int tankId)
        {
            if (!m_TankFactory.TryGetTank(tankId, out TankBrain tankBrain))
            {
                if (!m_TankFactory.TryGetRandomTank(out tankBrain))
                {
                    Debug.Log("Failed to spawn enemy tank");
                    return;
                }
            }

            tankBrain.Model.IsPlayer = false;
            tankBrain.SetLayers(m_EnemyLayer, m_PlayerLayer);

            m_EnemyController = m_EnemyPool.GetEnemy();
            m_EnemyController.SetTankBrain(tankBrain);

            Pose pose = m_EnemyWaves.GetRandomSpawnPose();
            m_EnemyController.SetPose(pose);
            m_EnemyController.Init();
        }*/

        private void OnTankAboutToDie(bool isPlayer)
        {
            if (isPlayer)
                return;

            TryStartNextWave();
        }

        private void TryStartNextWave()
        {
            m_TankCountInCurrentWave--;
            if (m_TankCountInCurrentWave > 0)
            {
                return;
            }

            m_NextWaveIndex++;

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
