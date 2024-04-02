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

        private EnemyController m_EnemyAI;

        private int m_PlayerLayer;
        private int m_EnemyLayer;

        public EnemyService(
            TankFactory tankFactory, 
            WaveConfigSO enemyWaves,
            int playerLayer,
            int enemyLayer)
        {
            m_Cts = new CancellationTokenSource();
            m_TankFactory = tankFactory;
            EventService.Instance.OnTankDead.AddListener(OnTankDead);
            m_EnemyWaves = enemyWaves;

            m_PlayerLayer = playerLayer;
            m_EnemyLayer = enemyLayer;
            m_NextWaveIndex = 0;
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
            if (!m_EnemyWaves.GetTanksForNextWave(m_NextWaveIndex, out int[] tankIDs))
            {
                Debug.Log("No tanks found!");
                return;
            }

            m_TankCountInCurrentWave = tankIDs.Length;

            foreach (int tankId in tankIDs)
            {
                CreateEnemyTank(tankId);
            }
        }

        public void CreateEnemyTank(int tankId)
        {
            if (!m_TankFactory.TryGetTank(tankId, out TankBrain controller))
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
            controller.SetLayers(m_EnemyLayer, m_PlayerLayer);

            m_EnemyAI = new EnemyController(controller);

            controller.SubscribeToFullyChargedEvent(m_EnemyAI.OnUltimateFullyCharged);
        }

        private void OnTankDead(bool isPlayer)
        {
            if (isPlayer)
                return;

            OnEnemyTankDead();
        }

        private void OnEnemyTankDead()
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
