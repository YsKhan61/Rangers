using BTG.Entity;
using BTG.Utilities;
using System.Threading;
using UnityEngine;
using VContainer;


namespace BTG.Enemy
{
    /// <summary>
    /// The service that manages everything related to the enemies of the game.
    /// It is responsible for spawning the enemies, starting the waves, and keeping track of the enemy death count.
    /// </summary>
    public class EnemyService
    {
        [Inject]
        private IObjectResolver m_ObjectResolver;

        [Inject]
        EntityFactoryContainerSO m_EntityFactoryContainer;

        [Inject]
        private IntDataSO m_EnemyDeathCountData;

        [Inject]
        private WaveConfigSO m_EnemyWaves;

        private CancellationTokenSource m_Cts;
        private EnemyPool m_EnemyPool;

        private int m_NextWaveIndex = 0;
        private int m_TankCountInCurrentWave = 0;


        public EnemyService()
        {
            m_Cts = new CancellationTokenSource();
        }

        ~EnemyService()
        {
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_Cts);
        }

        /// <summary>
        /// This method initializes the enemy service.
        /// It creates the enemy pool and starts the first wave.
        /// </summary>
        public void Initialize()
        {
            m_NextWaveIndex = 0;

            m_EnemyPool = new EnemyPool();
            m_ObjectResolver.Inject(m_EnemyPool);

            StartNextWaveWithEntityTags();
        }

        /// <summary>
        /// When an enemy dies, this method is called by the enemy controller of the enemy that died.
        /// </summary>
        public void OnEnemyDeath()
        {
            m_EnemyDeathCountData.Value++;
            TryStartNextWave();
        }

        /// <summary>
        /// Starts the next wave with the entity tags of the next wave.
        /// </summary>
        public void StartNextWaveWithEntityTags()
        {
            if (!m_EnemyWaves.TryGetEntityTagsForNextWave(m_NextWaveIndex, out TagSO[] tags))
            {
                Debug.Log("No entity found!");
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
            

            controller.SetEntityBrain(entity);
            controller.Init();
            Pose pose = m_EnemyWaves.GetRandomSpawnPose();
            controller.SetPose(pose);  
        }

        private bool TryGetEntity(TagSO tag, out IEntityBrain entity)
        {
            entity = m_EntityFactoryContainer.GetFactory(tag).GetItem();
            return entity != null;
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
