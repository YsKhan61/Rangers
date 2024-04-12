using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "AirStrike Factory", menuName = "ScriptableObjects/UltimateActionFactory/AirStrikeFactorySO")]
    public class AirStrikeFactorySO : UltimateActionFactorySO
    {
        [SerializeField] private AirStrikeDataSO m_AirStrikeData;

        public override IUltimateAction CreateUltimateAction(IUltimateActor actor)
        {
            return new AirStrike(actor, m_AirStrikeData);
        }
    }
}


