using BTG.Factory;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "Auto Target Factory", menuName = "ScriptableObjects/Factory/UltimateActionFactory/AutoTargetFactorySO")]
    public class AutoTargetFactorySO : UltimateActionFactorySO
    {
        [SerializeField]
        AutoTargetDataSO m_AutoTargetData;

        public override IUltimateAction GetItem() => new AutoTarget(m_AutoTargetData);

        public override IUltimateAction GetNetworkItem() => new NetworkAutoTarget(m_AutoTargetData); 
    }
}

