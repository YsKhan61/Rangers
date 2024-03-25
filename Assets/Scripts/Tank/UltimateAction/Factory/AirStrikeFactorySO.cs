using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "AirStrike Factory", menuName = "ScriptableObjects/UltimateActionFactory/AirStrikeFactorySO")]
    public class AirStrikeFactorySO : UltimateActionFactorySO
    {
        [SerializeField] private AirStrikeDataSO m_AirStrikeData;

        public override IUltimateAction CreateUltimateAction(TankUltimateController controller)
        {
            return new AirStrike(controller, m_AirStrikeData);
        }
    }
}


