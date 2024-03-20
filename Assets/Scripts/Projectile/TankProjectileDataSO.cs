using UnityEngine;

[CreateAssetMenu(fileName = "TankProjectileData", menuName = "ScriptableObjects/TankProjectileDataSO")]
public class TankProjectileDataSO : ScriptableObject
{
    [SerializeField] private TankProjectileView m_ProjectileViewPrefab;
    public TankProjectileView ProjectileViewPrefab => m_ProjectileViewPrefab;
}
