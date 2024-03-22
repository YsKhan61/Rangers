using UnityEngine;


namespace BTG.Tank.Projectile
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileDataSO")]
    public class ProjectileDataSO : ScriptableObject
    {
        [SerializeField] private ProjectileView m_ProjectileViewPrefab;
        public ProjectileView ProjectileViewPrefab => m_ProjectileViewPrefab;

        [SerializeField] private float m_MinInitialSpeed;
        public float MinInitialSpeed => m_MinInitialSpeed;

        [SerializeField] private float m_MaxInitialSpeed;
        public float MaxInitialSpeed => m_MaxInitialSpeed;
    }
}

