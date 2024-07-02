using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "AutoTargetData", menuName = "ScriptableObjects/UltimateAction/AutoTargetDataSO")]
    public class AutoTargetDataSO : UltimateActionDataSO
    {
        [SerializeField, Tooltip("The center of the impact area will be offset in the forward direction of the owner tank")]
        private int m_CenterOffset;
        public int CenterOffset => m_CenterOffset;

        [SerializeField, Tooltip("The radius of the impact area")]
        private int m_ImpactRadius;
        public int ImpactRadius => m_ImpactRadius;

        [SerializeField, Tooltip("The no of bullets to be fired per second")]
        private int m_FireRate;
        public int FireRate => m_FireRate;

        [SerializeField, Tooltip("This will be the visual")]
        private AutoTargetView m_AutoTargetViewPrefab;
        public AutoTargetView AutoTargetViewPrefab => m_AutoTargetViewPrefab;

        [SerializeField, Tooltip("This will be the actual network visual")]
        private AutoTargetView m_NetworkAutoTargetView;
        public AutoTargetView NetworkAutoTargetView => m_NetworkAutoTargetView;

        [SerializeField]
        private int m_ProjectileSpeed;
        public int ProjectileSpeed => m_ProjectileSpeed;

        [SerializeField]
        private int m_Damage;
        public int Damage => m_Damage;

        [SerializeField]
        private TagSO m_ExplosionTag;
        public TagSO ExplosionTag => m_ExplosionTag;

        [SerializeField, Tooltip("How long the bullets will stay following the target before self destroy!")]
        private int m_BulletDuration;
        public int BulletDuration => m_BulletDuration;

    }
}
