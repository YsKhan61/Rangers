using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "AirStrike", menuName = "ScriptableObjects/UltimateAction/AirStrikeDataSO")]
    public class AirStrikeDataSO : UltimateActionDataSO
    {
        [SerializeField] private float m_Duration;
        public float Duration => m_Duration;

        [SerializeField] private AirStrikeView m_AirStrikeViewPrefab;
        public AirStrikeView AirStrikeViewPrefab => m_AirStrikeViewPrefab;
    }
}
