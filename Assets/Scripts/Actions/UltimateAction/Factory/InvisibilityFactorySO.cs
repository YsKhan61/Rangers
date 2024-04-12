using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "Invisibility Factory", menuName = "ScriptableObjects/UltimateActionFactory/InvisibilityFactorySO")]
    public class InvisibilityFactorySO : UltimateActionFactorySO
    {
        [SerializeField] private InvisibilityDataSO m_InvisibilityData;

        public override IUltimateAction CreateUltimateAction(IUltimateActor actor)
        {
            return new Invisibility(actor, m_InvisibilityData);
        }
    }

}
