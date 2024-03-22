using UnityEngine;


namespace BTG.Tank.Projectile
{
    [CreateAssetMenu(fileName = "TankProjectileData", menuName = "ScriptableObjects/TankProjectileDataSO")]
    public class TankProjectileDataSO : ScriptableObject
    {
        [SerializeField] private TankProjectileView m_ProjectileViewPrefab;
        public TankProjectileView ProjectileViewPrefab => m_ProjectileViewPrefab;

        [SerializeField] private float m_MinInitialSpeed;
        public float MinInitialSpeed => m_MinInitialSpeed;

        [SerializeField] private float m_MaxInitialSpeed;
        public float MaxInitialSpeed => m_MaxInitialSpeed;
    }
}

