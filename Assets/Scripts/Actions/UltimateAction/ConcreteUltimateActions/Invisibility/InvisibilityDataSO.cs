using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "Invisibility", menuName = "ScriptableObjects/UltimateAction/InvisibilityDataSO")]
    public class InvisibilityDataSO : UltimateActionDataSO
    {
        [SerializeField, Tooltip("Execution duration")] 
        private int m_Duration;
        public int Duration => m_Duration;

        [SerializeField] private InvisibilityView m_InvisibilityViewPrefab;
        public InvisibilityView InvisibilityViewPrefab => m_InvisibilityViewPrefab;
    }
}
