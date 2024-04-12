using BTG.Actions.UltimateAction;
using BTG.Tank.Projectile;
using UnityEngine;


namespace BTG.Tank
{
    [CreateAssetMenu(fileName = "TankData", menuName = "ScriptableObjects/TankDataSO")]
    public class TankDataSO : ScriptableObject
    {
        [SerializeField]
        private int m_ID;
        public int ID => m_ID;

        [SerializeField]
        private Sprite m_Icon;
        public Sprite Icon => m_Icon;

        [SerializeField] 
        private TankView m_TankViewPrefab;
        public TankView TankViewPrefab => m_TankViewPrefab;

        [SerializeField]
        private int m_MaxHealth;
        public int MaxHealth => m_MaxHealth;

        [SerializeField] 
        private float m_Acceleration;
        public float Acceleration => m_Acceleration;

        [SerializeField] 
        private int m_MaxSpeed;
        public int MaxSpeed => m_MaxSpeed;

        [SerializeField] 
        private int m_RotateSpeed;
        public int RotateSpeed => m_RotateSpeed;

        [SerializeField] 
        private ProjectileDataSO m_projectileData;
        public ProjectileDataSO ProjectileData => m_projectileData;

        [SerializeField] 
        private int m_ChargeTime;
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

        
    }
}