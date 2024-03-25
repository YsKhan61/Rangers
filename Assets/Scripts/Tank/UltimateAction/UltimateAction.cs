
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace BTG.Tank.UltimateAction
{
    public abstract class UltimateAction : IUltimateAction
    {
        public const int FULL_CHARGE = 100;

        public event System.Action<string> OnUltimateActionAssigned;
        public event System.Action<int> OnChargeUpdated;
        public event System.Action OnFullyCharged;
        
        public event System.Action OnUltimateActionExecuted;

        protected UltimateActionDataSO m_UltimateActionData;
        protected CancellationTokenSource m_CancellationTokenSource;

        private float m_ChargedAmount;


        public string Name => m_UltimateActionData.name;

        public float Duration => m_UltimateActionData.Duration;

        public float ChargeRate => m_UltimateActionData.ChargeRate;

        public bool IsFullyCharged => m_ChargedAmount >= FULL_CHARGE;

        public void AutoCharge()
        {
            _ = AutoChargeAsync(m_CancellationTokenSource.Token);
        }

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
            m_CancellationTokenSource.Cancel();
            OnUltimateActionAssigned = null;
            OnChargeUpdated = null;
            OnFullyCharged = null;
        }

        protected virtual void Start()
        {
            m_CancellationTokenSource = new CancellationTokenSource();

            Charge(-FULL_CHARGE);

            RaiseUltimateActionAssignedEvent();
        }

        protected async void RaiseUltimateActionAssignedEvent()
        {
            // wait for 1 frame to ensure that the event is subscribed to
            await Task.Yield();
            OnUltimateActionAssigned?.Invoke(Name);
        }

        protected void RaiseFullyChargedEvent()
        {
            OnFullyCharged?.Invoke();
        }

        protected void RaiseUltimateActionExecutedEvent()
        {
            OnUltimateActionExecuted?.Invoke();
        }

        protected async Task ResetAfterDuration(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay((int)(m_UltimateActionData.Duration * 1000), cancellationToken);
                Reset();
            }
            catch (TaskCanceledException)
            {
                // Do nothing
            }
        }

        protected abstract void Reset();

        private async Task AutoChargeAsync(CancellationToken token)
        {
            try
            {
                while (!IsFullyCharged)
                {
                    Charge(ChargeRate);
                    await Task.Delay(1000, token);
                }
            }
            catch (TaskCanceledException)
            {
                // Task was cancelled
            }
        }
    }
}
