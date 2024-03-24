using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "InvisibilityData", menuName = "ScriptableObjects/UltimateAction/InvisibilityDataSO")]
    public class InvisibilityDataSO : UltimateActionDataSO
    {
        [SerializeField] private InvisibilityView m_InvisibilityViewPrefab;
        public InvisibilityView InvisibilityViewPrefab => m_InvisibilityViewPrefab;
    }
}
