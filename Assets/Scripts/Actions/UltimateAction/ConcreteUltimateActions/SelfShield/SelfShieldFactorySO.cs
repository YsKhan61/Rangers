using BTG.Factory;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "Self Shield Factory", menuName = "ScriptableObjects/Factory/UltimateActionFactory/SelfShieldFactorySO")]
    public class SelfShieldFactorySO : FactorySO<IUltimateAction>
    {
        [SerializeField]
        SelfShieldDataSO m_SelfShieldData;

        public override IUltimateAction GetItem() => new SelfShield(m_SelfShieldData);
    }
}

