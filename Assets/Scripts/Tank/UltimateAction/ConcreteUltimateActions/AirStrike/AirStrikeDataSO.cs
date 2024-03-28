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

        [SerializeField] private int m_Damage;
        public int Damage => m_Damage;

        [SerializeField] private float m_Radius;
        public float ImpactRadius => m_Radius;
    }
}
