using UnityEngine;


namespace BTG.Enemy
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "ScriptableObjects/WaveConfigSO")]
    public class WaveConfigSO : ScriptableObject
    {
        [SerializeField, Tooltip("Interval between each waves")] 
        private int m_Interval = 3;
        public int Interval => m_Interval;

        [SerializeField] private Wave[] m_Waves;

        [SerializeField] private Pose[] m_SpawnPose;

        private int m_NextWaveIndex = 0;        // start from 0

        public int TankCountInCurrentWave;

        public bool GetTanksForNextWave(out int[] tankIDs)
        {
            if (m_NextWaveIndex >= m_Waves.Length)
            {
                tankIDs = null;
                return false;
            }

            tankIDs = m_Waves[m_NextWaveIndex].EnemyTankIDs;
            TankCountInCurrentWave = tankIDs.Length;
            m_NextWaveIndex++;
            return true;
        }

        public Pose GetRandomSpawnPose()
        {
            return m_SpawnPose[Random.Range(0, m_SpawnPose.Length)];
        }
    }


    [System.Serializable]
    public class Wave
    {
        public int ID;
        public int[] EnemyTankIDs;
    }

}
