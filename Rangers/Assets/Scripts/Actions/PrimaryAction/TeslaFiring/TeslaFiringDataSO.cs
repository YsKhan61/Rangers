using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{

    [CreateAssetMenu(fileName = "TeslaFiringData", menuName = "ScriptableObjects/PrimaryAction/TeslaFiringDataSO")]
    public class TeslaFiringDataSO : PrimaryActionDataSO
    {
        [SerializeField, Tooltip("This prefab is used in single player")]
        TeslaBallView m_TeslaBallViewPrefab;
        public TeslaBallView TeslaBallViewPrefab => m_TeslaBallViewPrefab;

        [SerializeField, Tooltip("This prefab is used in multiplayer")]
        NetworkTeslaBallView m_NetworkTeslaBallViewPrefab;
        public NetworkTeslaBallView NetworkTeslaBallViewPrefab => m_NetworkTeslaBallViewPrefab;

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

        [SerializeField, Tooltip("The tag that will be used to send shoot effect event data")]
        private TagSO m_ShootEffectTag;
        public TagSO ShootEffectTag => m_ShootEffectTag;

        [SerializeField, Tooltip("The tag that will be used to send hit effect event data")]
        private TagSO m_HitEffectTag;
        public TagSO HitEffectTag => m_HitEffectTag;
    }
}

