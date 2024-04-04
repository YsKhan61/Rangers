using UnityEngine;


namespace BTG.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyDataSO")]
    public class EnemyDataSO : ScriptableObject
    {
        [SerializeField]
        private Vector3[] m_PatrolPoints;
        public Vector3[] PatrolPoints => m_PatrolPoints;

        [SerializeField]
        private EnemyView m_EnemyPrefab;
        public EnemyView EnemyPrefab => m_EnemyPrefab;

        [SerializeField, Tooltip("The accepted distance between tank and destination to consider reached")]
        private float m_DestinationReachedThreshold = 0.5f;
        public float DestinationReachedThreshold => m_DestinationReachedThreshold;

        [SerializeField]
        private int m_LookAtSpeed = 5;
        public int LookAtSpeed => m_LookAtSpeed;
    }
}
