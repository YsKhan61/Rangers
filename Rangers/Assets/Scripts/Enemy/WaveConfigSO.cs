using BTG.Utilities;
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


        public bool GetTanksForNextWave(int index, out int[] tankIDs)
        {
            tankIDs = null;
            if (index >= m_Waves.Length)
            {
                return false;
            }

            tankIDs = m_Waves[index].EnemyTankIDs;
            if (tankIDs == null)
            {
                return false;
            }

            return true;
        }

        public bool TryGetEntityTagsForNextWave(int index, out TagSO[] tags) 
        {
            tags = null;
            if (index < 0 || index >= m_Waves.Length) return false;

            tags = m_Waves[index].EntityTags;
            if (tags == null) return false;

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
        public TagSO[] EntityTags;
        public int[] EnemyTankIDs;
    }

}
