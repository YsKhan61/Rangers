using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "SelfShield", menuName = "ScriptableObjects/UltimateAction/SelfShieldDataSO")]
    public class SelfShieldDataSO : UltimateActionDataSO
    {
        [SerializeField] private SelfShieldView m_SelfShieldViewPrefab;
        public SelfShieldView SelfShieldViewPrefab => m_SelfShieldViewPrefab;
    }
}
