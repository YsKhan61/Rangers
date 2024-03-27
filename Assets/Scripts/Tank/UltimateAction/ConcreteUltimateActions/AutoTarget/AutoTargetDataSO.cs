using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "AutoTargetData", menuName = "ScriptableObjects/UltimateAction/AutoTargetDataSO")]
    public class AutoTargetDataSO : UltimateActionDataSO
    {
        [SerializeField]
        private LayerMask m_TargetLayer;
        public LayerMask TargetLayer => m_TargetLayer;

        [SerializeField, Tooltip("The center of the impact area will be offset in the forward direction of the owner tank")]
        private int m_CenterOffset;
        public int CenterOffset => m_CenterOffset;

        [SerializeField, Tooltip("The radius of the impact area")]
        private int m_ImpactRadius;
        public int ImpactRadius => m_ImpactRadius;

        [SerializeField, Tooltip("The no of bullets to be fired per second")]
        private int m_FireRate;
        public int FireRate => m_FireRate;

        [SerializeField, Tooltip("This will be the actual projectile")]
        private AutoTargetView m_AutoTargetViewPrefab;
        public AutoTargetView AutoTargetViewPrefab => m_AutoTargetViewPrefab;

        [SerializeField]
        private int m_ProjectileSpeed;
        public int ProjectileSpeed => m_ProjectileSpeed;
    }
}