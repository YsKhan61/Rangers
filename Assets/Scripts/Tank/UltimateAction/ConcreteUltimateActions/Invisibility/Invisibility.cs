using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class Invisibility : UltimateAction
    {
        private InvisibilityDataSO m_InvisibilityData => m_UltimateActionData as InvisibilityDataSO;

        public Invisibility(InvisibilityDataSO invisibilityData)
        {
            m_UltimateActionData = invisibilityData;
        }

        public override bool TryExecute(TankUltimateController controller)
        {
            Debug.Log("Ultimate: Invisibility executed");
            return true;
        }

        public override void OnDestroy()
        {
            Debug.Log("Ultimate: Invisibility destroyed");
        }
    }
}
