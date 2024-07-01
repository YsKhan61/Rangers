using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    [CreateAssetMenu(fileName = "ChargedFiringData", menuName = "ScriptableObjects/PrimaryAction/ChargedFiringDataSO")]
    public class ChargedFiringDataSO : PrimaryActionDataSO
    {
        [SerializeField, Tooltip("This view is for singleplayer")]
        ProjectileView m_ViewPrefab;
        public ProjectileView ViewPrefab => m_ViewPrefab;

        [SerializeField, Tooltip("The view for multiplayer")]
        NetworkProjectileView m_NetworkViewPrefab;
        public NetworkProjectileView NetworkViewPrefab => m_NetworkViewPrefab;

        [SerializeField, Tooltip("Damage done by the projectile")]
        int m_Damage;
        public int Damage => m_Damage;

        [SerializeField, Tooltip("Minimum initial speed of the projectile at least charge")]
        float m_MinInitialSpeed;
        public float MinInitialSpeed => m_MinInitialSpeed;

        [SerializeField, Tooltip("Maximum initial speed of the projectile at most charge")]
        float m_MaxInitialSpeed;
        public float MaxInitialSpeed => m_MaxInitialSpeed;

        [SerializeField, Tooltip("The tag that will be used to send shoot effect event data")]
        private TagSO m_ShootEffectTag;
        public TagSO ShootEffectTag => m_ShootEffectTag;

        [SerializeField, Tooltip("The tag that will be used to send hit effect event data")]
        private TagSO m_HitEffectTag;
        public TagSO HitEffectTag => m_HitEffectTag;
    }
}

