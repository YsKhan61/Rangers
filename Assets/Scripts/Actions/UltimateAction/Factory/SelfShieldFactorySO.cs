using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "Self Shield Factory", menuName = "ScriptableObjects/UltimateActionFactory/SelfShieldFactorySO")]
    public class SelfShieldFactorySO : UltimateActionFactorySO
    {
        [SerializeField] private SelfShieldDataSO m_SelfShieldData;

        public override IUltimateAction CreateUltimateAction(IUltimateActor actor)
        {
            return new SelfShield(actor, m_SelfShieldData);
        }
    }
}

