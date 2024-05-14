using UnityEngine;


namespace BTG.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyDataSO")]
    public class EnemyDataSO : ScriptableObject
    {
        [SerializeField, Tooltip("The layer that will be used to mark the damage collider of player")]
        private int m_SelfLayer;
        public int SelfLayer => m_SelfLayer;

        [SerializeField, Tooltip("The layer that will be used to mark the damage collider of enemy")]
        private int m_OppositionLayer;
        public int OppositionLayerMask => 1 << m_OppositionLayer;

        [SerializeField]
        private Vector3[] m_PatrolPoints;
        public Vector3[] PatrolPoints => m_PatrolPoints;

        [SerializeField]
        private EnemyView m_EnemyPrefab;
        public EnemyView EnemyPrefab => m_EnemyPrefab;

        [SerializeField, Tooltip("The accepted distance between tank and destination to consider reached")]
        private float m_StoppingDistance = 0.5f;
        public float StoppingDistance => m_StoppingDistance;

        [SerializeField]
        private float m_MaxSpeedMultiplier = 0.5f;
        public float MaxSpeedMultiplier => m_MaxSpeedMultiplier;

        [SerializeField]
        private EnemyTankState m_InitialState;
        public EnemyTankState InitialState => m_InitialState;

#if UNITY_EDITOR
        /// <summary>
        /// Whether to initialize the state machine on start
        /// </summary>
        [SerializeField]
        private bool m_InitializeState = false;
        public bool InitializeState => m_InitializeState;
#endif
    }
}
