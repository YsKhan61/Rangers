using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "ScriptableObjects/TankDataSO")]
public class TankDataSO : ScriptableObject
{
    [SerializeField]
    private int m_ID;
    public int ID => m_ID;

    [SerializeField] private TankView m_TankViewPrefab;
    public TankView TankViewPrefab => m_TankViewPrefab;

    [SerializeField] private float m_MoveSpeed;
    public float MoveSpeed => m_MoveSpeed;

    [SerializeField] private float m_RotateSpeed;
    public float RotateSpeed => m_RotateSpeed;

    [SerializeField] private TankProjectileDataSO m_projectileData;
    public TankProjectileDataSO ProjectileData => m_projectileData;
}
