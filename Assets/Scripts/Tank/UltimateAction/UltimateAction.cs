
using UnityEngine;

namespace BTG.Tank.UltimateAction
{
    public abstract class UltimateAction : IUltimateAction
    {
        public const int FULL_CHARGE = 100;

        public event System.Action<string> OnUltimateActionAssigned;
        public event System.Action<int> OnChargeUpdated;
        public event System.Action OnFullyCharged;
        public event System.Action<float> OnUltimateExecuted;

        protected UltimateActionDataSO m_UltimateActionData;
        private float m_ChargedAmount;

        public string Name => m_UltimateActionData.name;

        public float Duration => m_UltimateActionData.Duration;

        public float ChargeRate => m_UltimateActionData.ChargeRate;

        public bool IsFullyCharged => m_ChargedAmount >= FULL_CHARGE;

        public virtual void Charge(float amount)
        {
            m_ChargedAmount += amount;
            m_ChargedAmount = Mathf.Clamp(m_ChargedAmount, 0, FULL_CHARGE);
            OnChargeUpdated?.Invoke((int)m_ChargedAmount);

            if (IsFullyCharged)
            {
                RaiseFullyChargedEvent();
            }
        }

        public abstract bool TryExecute(TankUltimateController controller);

        public virtual void OnDestroy()
        {
            OnUltimateActionAssigned = null;
            OnChargeUpdated = null;
            OnFullyCharged = null;
            OnUltimateExecuted = null;
        }

        protected void RaiseUltimateActionAssignedEvent()
        {
            OnUltimateActionAssigned?.Invoke(Name);
        }

        protected void RaiseFullyChargedEvent()
        {
            OnFullyCharged?.Invoke();
        }

        protected void RaiseUltimateExecutedEvent(float duration)
        {
            OnUltimateExecuted?.Invoke(duration);
        }
    }
}
