using UnityEngine;

namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "Auto Target Factory", menuName = "ScriptableObjects/UltimateActionFactory/AutoTargetFactorySO")]
    public class AutoTargetFactorySO : UltimateActionFactorySO
    {
        [SerializeField] private AutoTargetDataSO m_AutoTargetData;

        public override IUltimateAction CreateUltimateAction()
        {
            return new AutoTarget(m_AutoTargetData);
        }
    }
}

