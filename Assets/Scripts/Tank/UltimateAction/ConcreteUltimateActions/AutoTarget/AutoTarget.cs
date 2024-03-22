using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class AutoTarget : UltimateAction
    {
        private AutoTargetDataSO m_AutoTargetData => m_UltimateActionData as AutoTargetDataSO;

        public AutoTarget(AutoTargetDataSO autoTargetData)
        {
            m_UltimateActionData = autoTargetData;
        }

        public override bool TryExecute(TankUltimateController controller)
        {
            Debug.Log("Ultimate: Auto Target executed");
            return true;
        }

        public override void OnDestroy()
        {
            Debug.Log("Ultimate: Auto Target destroyed");
        }
    }
}
