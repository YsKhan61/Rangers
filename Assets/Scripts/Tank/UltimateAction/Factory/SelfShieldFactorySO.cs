using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "Self Shield Factory", menuName = "ScriptableObjects/UltimateActionFactory/SelfShieldFactorySO")]
    public class SelfShieldFactorySO : UltimateActionFactorySO
    {
        [SerializeField] private SelfShieldDataSO m_SelfShieldData;

        public override IUltimateAction CreateUltimateAction()
        {
            return new SelfShield(m_SelfShieldData);
        }
    }
}

