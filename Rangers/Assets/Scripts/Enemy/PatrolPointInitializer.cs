using UnityEngine;

namespace BTG.Enemy
{
    public class PatrolPointInitializer : MonoBehaviour
    {
        [SerializeField]
        private Transform[] m_PatrolPoints;

        [SerializeField]
        private EnemyDataSO m_EnemyData;

        private void Awake()
        {
            if (m_PatrolPoints.Length == 0)
            {
                Debug.LogError("No patrol points assigned to " + name);
                return;
            }

            Vector3[] points = new Vector3[m_PatrolPoints.Length];
            for (int i = 0; i < m_PatrolPoints.Length; i++)
            {
                if (m_PatrolPoints[i] == null)
                {
                    Debug.LogError("Patrol point " + i + " is not assigned to " + name);
                    return;
                }
                points[i] = m_PatrolPoints[i].position;
            }
        }
    }
}