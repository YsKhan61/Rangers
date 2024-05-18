using BTG.Effects;
using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{

    [CreateAssetMenu(fileName = "TeslaFiringData", menuName = "ScriptableObjects/PrimaryAction/TeslaFiringDataSO")]
    public class TeslaFiringDataSO : ScriptableObject
    {
        [SerializeField]
        TagSO m_Tag;
        public TagSO Tag => m_Tag;

        [SerializeField]
        TeslaBallView m_TeslaBallViewPrefab;
        public TeslaBallView TeslaBallViewPrefab => m_TeslaBallViewPrefab;

        [SerializeField]
        int m_ChargeTime;
        public int ChargeTime => m_ChargeTime;

        [SerializeField]
        int m_MinInitialSpeed;
        public int MinInitialSpeed => m_MinInitialSpeed;

        [SerializeField]
        int m_MaxInitialSpeed;
        public int MaxInitialSpeed => m_MaxInitialSpeed;

        [SerializeField, Tooltip("Min scale of the tesla ball")]
        float m_MinScale;
        public float MinTeslaBallScale => m_MinScale;

        [SerializeField, Tooltip("Max scale of the tesla ball")]
        float m_MaxScale;
        public float MaxTeslaBallScale => m_MaxScale;


        [SerializeField, Tooltip("Min Damage done by the Tesla Ball")]
        int m_MinDamage;
        public int MinDamage => m_MinDamage;

        [SerializeField, Tooltip("Max Damage done by the Tesla Ball")]
        int m_MaxDamage;
        public int MaxDamage => m_MaxDamage;

        [SerializeField]
        AudioClip m_ShotFiredClip;
        public AudioClip ShotFiredClip => m_ShotFiredClip;

        [SerializeField, Tooltip("The charging sound clip")]
        private AudioClip m_ChargingClip;
        public AudioClip ChargingClip => m_ChargingClip;

        [SerializeField]
        ExplosionFactorySO m_ExplosionFactory;
        public ExplosionFactorySO ExplosionFactory => m_ExplosionFactory;
    }
}

