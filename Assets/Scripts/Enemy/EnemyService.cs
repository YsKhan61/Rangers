using BTG.Entity;
using BTG.Tank;
using BTG.Utilities;
using BTG.Utilities.DI;
using System.Threading;
using UnityEngine;


namespace BTG.Enemy
{
    public class EnemyService
    {
        [Inject]
        EntityFactoryContainerSO m_FactoryContainer;

        [Inject]
        private IntDataSO m_EnemyDeathCountData;

        [Inject]
        private WaveConfigSO m_EnemyWaves;

        private CancellationTokenSource m_Cts;

        private int m_NextWaveIndex = 0;
        private int m_TankCountInCurrentWave = 0;

        private EnemyPool m_EnemyPool;
        

        public EnemyService()
        {
            m_Cts = new CancellationTokenSource();
        }

        ~EnemyService()
        {
            // m_TankFactory = null;

            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        [Inject]
        public void Initialize()
        {
            m_NextWaveIndex = 0;
            m_EnemyPool = new EnemyPool();
            DIManager.Instance.Inject(m_EnemyPool);
        }

        public void OnEnemyDeath()
        {
            m_EnemyDeathCountData.Value++;
            TryStartNextWave();
        }

        public void StartNextWaveWithEntityTags()
        {
            if (!m_EnemyWaves.TryGetEntityTagsForNextWave(m_NextWaveIndex, out TagSO[] tags))
            {
                Debug.Log("No tanks found!");
                return;
            }

            foreach (TagSO tag in tags)
            {
                GetEntityAndConfigureWithController(tag);
            }
        }

        private void GetEntityAndConfigureWithController(TagSO tag)
        {
            bool entityFound = TryGetEntity(tag, out IEntityBrain entity);
            if (!entityFound) return;

            bool controllerFound = GetEnemyController(out EnemyController controller);
            if (!controllerFound) return;

            controller.SetTank(entity as TankBrain);
            controller.SetService(this);
            Pose pose = m_EnemyWaves.GetRandomSpawnPose();
            controller.SetPose(pose);
            controller.Init();
        }

        private bool TryGetEntity(TagSO tag, out IEntityBrain entity)
        {
            entity = null;
            if (!m_FactoryContainer.TryGetFactory(tag, out EntityFactorySO factory))
            {
                Debug.Log("Failed to get entity factory of tag: " + tag.name);
                return false;
            }

            entity = factory.GetEntity();
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
                StartNextWaveWithEntityTags();
            }, m_Cts.Token);
        }
    }
}
