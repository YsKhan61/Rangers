using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "AirStrike Factory", menuName = "ScriptableObjects/UltimateActionFactory/AirStrikeFactorySO")]
    public class AirStrikeFactorySO : UltimateActionFactorySO
    {
        [SerializeField] private AirStrikeDataSO m_AirStrikeData;

        public override IUltimateAction CreateUltimateAction()
        {
            return new AirStrike(m_AirStrikeData);
        }
    }
}


