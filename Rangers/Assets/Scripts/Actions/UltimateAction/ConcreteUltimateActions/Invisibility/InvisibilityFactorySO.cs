using BTG.Factory;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "Invisibility Factory", menuName = "ScriptableObjects/Factory/UltimateActionFactory/InvisibilityFactorySO")]
    public class InvisibilityFactorySO : FactorySO<IUltimateAction>
    {
        [SerializeField]
        InvisibilityDataSO m_InvisibilityData;

        public override IUltimateAction GetItem() => new Invisibility(m_InvisibilityData);
    }

}
