using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "AirStrikeData", menuName = "ScriptableObjects/UltimateAction/AirStrikeDataSO")]
    public class AirStrikeDataSO : ScriptableObject
    {
        [SerializeField] private AirStrikeView m_AirStrikeViewPrefab;
        public AirStrikeView AirStrikeViewPrefab => m_AirStrikeViewPrefab;

        [SerializeField] private float m_Duration;
        public float Duration => m_Duration;
    }
}
