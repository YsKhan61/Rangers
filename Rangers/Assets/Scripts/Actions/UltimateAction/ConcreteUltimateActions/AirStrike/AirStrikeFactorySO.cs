using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// Factory for the AirStrike ultimate action
    /// This concrete class is needed to create a scriptable object asset in the project,
    /// as Unity does not support creating scriptable object assets from generic classes. (FactorySO<IUltimateAction>)
    /// </summary>
    [CreateAssetMenu(fileName = "AirStrike Factory", menuName = "ScriptableObjects/Factory/UltimateActionFactory/AirStrikeFactorySO")]
    public class AirStrikeFactorySO : UltimateActionFactorySO
    {
        [SerializeField]
        AirStrikeDataSO m_AirStrikeData;

        public override IUltimateAction GetItem() => new AirStrike(m_AirStrikeData);

        public override IUltimateAction GetNetworkItem() { return GetItem(); } // => new NetworkAirStrike(m_AirStrikeData);
    }
}