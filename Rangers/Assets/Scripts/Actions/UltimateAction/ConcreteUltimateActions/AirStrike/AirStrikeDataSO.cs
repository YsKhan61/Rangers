using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "AirStrike", menuName = "ScriptableObjects/UltimateAction/AirStrikeDataSO")]
    public class AirStrikeDataSO : UltimateActionDataSO
    {
        [SerializeField, Tooltip("Execution duration")] 
        private int m_Duration;
        public int Duration => m_Duration;

        [SerializeField] private int m_Damage;
        public int Damage => m_Damage;

        [SerializeField] private int m_Radius;
        public int ImpactRadius => m_Radius;
    }
}
