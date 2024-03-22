using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "AirStrikeData", menuName = "ScriptableObjects/UltimateAction/AirStrikeDataSO")]
    public class AirStrikeDataSO : UltimateActionDataSO
    {
        [SerializeField] private AirStrikeView m_AirStrikeViewPrefab;
        public AirStrikeView AirStrikeViewPrefab => m_AirStrikeViewPrefab;
    }
}
