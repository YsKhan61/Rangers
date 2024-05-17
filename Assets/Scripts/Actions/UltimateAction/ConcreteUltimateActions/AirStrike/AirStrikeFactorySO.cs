using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "AirStrike Factory", menuName = "ScriptableObjects/Factory/UltimateActionFactory/AirStrikeFactorySO")]
    public class AirStrikeFactorySO : UltimateActionFactorySO
    {
        [SerializeField]
        AirStrikeDataSO m_AirStrikeData;

        public override IUltimateAction CreateUltimateAction(IUltimateActor actor)
            => new AirStrike(actor, m_AirStrikeData);
    }
}


