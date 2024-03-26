using BTG.Tank.Projectile;
using BTG.Tank.UltimateAction;
using UnityEngine;
using UnityEngine.Serialization;

namespace BTG.Tank
{
    [CreateAssetMenu(fileName = "TankData", menuName = "ScriptableObjects/TankDataSO")]
    public class TankDataSO : ScriptableObject
    {
        [SerializeField]
        private int m_ID;
        public int ID => m_ID;

        [SerializeField] private TankView m_TankViewPrefab;
        public TankView TankViewPrefab => m_TankViewPrefab;

        [SerializeField, FormerlySerializedAs("m_MoveSpeed")] private float m_Acceleration;
        public float Acceleration => m_Acceleration;

        [SerializeField] private float m_MaxSpeed;
        public float MaxSpeed => m_MaxSpeed;

        [SerializeField] private float m_RotateSpeed;
        public float RotateSpeed => m_RotateSpeed;

        [SerializeField] private ProjectileDataSO m_projectileData;
        public ProjectileDataSO ProjectileData => m_projectileData;

        [SerializeField] private int m_ChargeTime;
        public int ChargeTime => m_ChargeTime;

        [SerializeField]
        private AudioClip m_EngineIdleClip;
        public AudioClip EngineIdleClip => m_EngineIdleClip;

        [SerializeField]
        private AudioClip m_EngineDrivingClip;
        public AudioClip EngineDrivingClip => m_EngineDrivingClip;

        [SerializeField]
        private AudioClip m_ShotFiringClip;
        public AudioClip ShotFiringClip => m_ShotFiringClip;

        [SerializeField]
        private AudioClip m_ShotChargingClip;
        public AudioClip ShotChargingClip => m_ShotChargingClip;

        [SerializeField]
        private UltimateActionFactorySO m_UltimateActionFactory;
        public UltimateActionFactorySO UltimateActionFactory => m_UltimateActionFactory;

        [SerializeField] private int m_MaxHealth;
        public int MaxHealth => m_MaxHealth;
    }
}


