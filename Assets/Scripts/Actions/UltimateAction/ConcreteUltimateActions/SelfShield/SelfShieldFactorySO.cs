using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "Self Shield Factory", menuName = "ScriptableObjects/Factory/UltimateActionFactory/SelfShieldFactorySO")]
    public class SelfShieldFactorySO : UltimateActionFactorySO
    {
        [SerializeField]
        SelfShieldDataSO m_SelfShieldData;

        public override IUltimateAction CreateUltimateAction(IUltimateActor actor)
            => new SelfShield(actor, m_SelfShieldData);
    }
}

