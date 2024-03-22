
namespace BTG.Tank.UltimateAction
{
    public abstract class UltimateAction : IUltimateAction
    {
        protected UltimateActionDataSO m_UltimateActionData;
        protected float m_ChargedAmount;

        public string Name => m_UltimateActionData.Name;

        public float Duration => m_UltimateActionData.Duration;

        public float ChargeRate => m_UltimateActionData.ChargeRate;

        public bool IsFullyCharged => m_UltimateActionData.ChargeRate >= IUltimateAction.FULL_CHARGE;

        public virtual void Charge(float amount)
        {
            if (!IsFullyCharged)
                m_ChargedAmount += amount;
        }

        public abstract bool TryExecute(TankUltimateController controller);

        public abstract void OnDestroy();
    }
}
