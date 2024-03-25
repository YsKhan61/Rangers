using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "Invisibility", menuName = "ScriptableObjects/UltimateAction/InvisibilityDataSO")]
    public class InvisibilityDataSO : UltimateActionDataSO
    {
        [SerializeField] private float m_Duration;
        public float Duration => m_Duration;

        [SerializeField] private InvisibilityView m_InvisibilityViewPrefab;
        public InvisibilityView InvisibilityViewPrefab => m_InvisibilityViewPrefab;
    }
}
