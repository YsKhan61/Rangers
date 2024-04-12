using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "Auto Target Factory", menuName = "ScriptableObjects/UltimateActionFactory/AutoTargetFactorySO")]
    public class AutoTargetFactorySO : UltimateActionFactorySO
    {
        [SerializeField] private AutoTargetDataSO m_AutoTargetData;

        public override IUltimateAction CreateUltimateAction(IUltimateActor actor)
        {
            return new AutoTarget(actor, m_AutoTargetData);
        }
    }
}

