using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "SelfShield", menuName = "ScriptableObjects/UltimateAction/SelfShieldDataSO")]
    public class SelfShieldDataSO : UltimateActionDataSO
    {
        [SerializeField, Tooltip("Execution duration")] 
        private int m_Duration;
        public int Duration => m_Duration;

        [SerializeField, Tooltip("This view is for single player mode.")] 
        private SelfShieldView m_SelfShieldViewPrefab;
        public SelfShieldView SelfShieldViewPrefab => m_SelfShieldViewPrefab;

        [SerializeField, Tooltip("This view is for multiplayer mode.")]
        private SelfShieldView m_NetworkSelfShieldViewPrefab;
        public SelfShieldView NetworkSelfShieldViewPrefab => m_NetworkSelfShieldViewPrefab;
    }
}
