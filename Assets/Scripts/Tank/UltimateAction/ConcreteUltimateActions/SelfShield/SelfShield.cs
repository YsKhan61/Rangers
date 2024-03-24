using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class SelfShield : UltimateAction
    {
        private SelfShieldDataSO m_SelfShieldData => m_UltimateActionData as SelfShieldDataSO;

        public SelfShield(SelfShieldDataSO selfShieldData)
        {
            m_UltimateActionData = selfShieldData;
            Start();
        }

        public override bool TryExecute(TankUltimateController controller)
        {
            Debug.Log("Ultimate: Self Shield executed");
            return true;
        }

        public override void OnDestroy()
        {
            Debug.Log("Ultimate: Self Shield destroyed");
        }

        protected override void Reset()
        {
            
        }
    }
}
