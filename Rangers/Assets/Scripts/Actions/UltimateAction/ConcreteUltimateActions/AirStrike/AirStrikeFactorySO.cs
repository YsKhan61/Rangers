using BTG.Factory;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "AirStrike Factory", menuName = "ScriptableObjects/Factory/UltimateActionFactory/AirStrikeFactorySO")]
    public class AirStrikeFactorySO : FactorySO<IUltimateAction>
    {
        [SerializeField]
        AirStrikeDataSO m_AirStrikeData;

        public override IUltimateAction GetItem() => new AirStrike(m_AirStrikeData);
    }
}


