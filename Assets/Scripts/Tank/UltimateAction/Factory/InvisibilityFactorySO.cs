using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    [CreateAssetMenu(fileName = "Invisibility Factory", menuName = "ScriptableObjects/UltimateActionFactory/InvisibilityFactorySO")]
    public class InvisibilityFactorySO : UltimateActionFactorySO
    {
        [SerializeField] private InvisibilityDataSO m_InvisibilityData;

        public override IUltimateAction CreateUltimateAction(TankUltimateController controller)
        {
            return new Invisibility(controller, m_InvisibilityData);
        }
    }

}