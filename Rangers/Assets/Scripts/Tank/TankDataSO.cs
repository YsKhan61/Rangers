using BTG.Entity;
using BTG.Utilities;
using UnityEngine;


namespace BTG.Tank
{
    [CreateAssetMenu(fileName = "TankData", menuName = "ScriptableObjects/TankDataSO")]
    public class TankDataSO : EntityDataSO
    {
        [SerializeField, Tooltip("The tag of the tank")]
        private TagSO m_Tag;
        public override TagSO Tag => m_Tag;

        [SerializeField]
        private Sprite m_Icon;
        public Sprite Icon => m_Icon;

        [SerializeField]
        private int m_CharSelectSeatIndex;
        public override int CharSelectSeatIndex => m_CharSelectSeatIndex;

        [SerializeField] 
        private TankView m_TankViewPrefab;
        public TankView TankViewPrefab => m_TankViewPrefab;

        [SerializeField]
        private GameObject m_Graphics;
        public override GameObject Graphics => m_Graphics;

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