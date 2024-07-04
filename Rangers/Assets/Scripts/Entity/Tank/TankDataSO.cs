using BTG.Entity;
using BTG.Utilities;
using UnityEngine;


namespace BTG.Entity.Tank
{
    [CreateAssetMenu(fileName = "TankData", menuName = "ScriptableObjects/TankDataSO")]
    public class TankDataSO : EntityDataSO
    {
        [SerializeField]
        private Sprite m_Icon;
        public override Sprite Icon => m_Icon;

        [SerializeField]
        private int m_CharSelectSeatIndex;
        public override int CharSelectSeatIndex => m_CharSelectSeatIndex;

        [SerializeField] 
        private TankView m_TankViewPrefab;
        public TankView TankViewPrefab => m_TankViewPrefab;
        public override GameObject Graphics => m_TankViewPrefab.gameObject;

        [SerializeField]
        private int m_MaxHealth;
        public override int MaxHealth => m_MaxHealth;

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

        [SerializeField, Tooltip("The tag of the primary action")]
        private TagSO m_PrimaryActionTag;
        public TagSO PrimaryTag => m_PrimaryActionTag;

        [SerializeField, Tooltip("The tag of the ultimate action")]
        private TagSO m_UltimateActionTag;
        public TagSO UltimateTag => m_UltimateActionTag;

        [SerializeField, Tooltip("The death sound clip")]
        private AudioClip m_DeathSoundClip;
        public AudioClip DeathSoundClip => m_DeathSoundClip;

    }
}