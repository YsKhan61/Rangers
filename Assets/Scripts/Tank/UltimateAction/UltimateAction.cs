
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using State = BTG.Tank.UltimateAction.IUltimateAction.State;

namespace BTG.Tank.UltimateAction
{
    public abstract class UltimateAction : IUltimateAction
    {
        public const int FULL_CHARGE = 100;

        public event System.Action<string> OnUltimateActionAssigned;
        public abstract event System.Action<IUltimateAction> OnFullyCharged;
        public event System.Action<int> OnChargeUpdated;
        public event System.Action OnUltimateActionExecuted;

        protected TankUltimateController m_UltimateController;
        protected UltimateActionDataSO m_UltimateActionData;
        protected CancellationTokenSource m_CancellationTokenSource;

        private float m_ChargedAmount;


        public string Name => m_UltimateActionData.name;

        public float ChargeRate => m_UltimateActionData.ChargeRate;

        public State CurrentState { get; protected set; }
        // public bool IsFullyCharged => m_ChargedAmount >= FULL_CHARGE;

        public void ChangeState(State newState)
        {
            CurrentState = newState;
        }

        public void AutoCharge()
        {
            _ = AutoChargeAsync(m_CancellationTokenSource.Token);
        }

        public virtual void Charge(float amount)
        {
            if (CurrentState != State.Charging)
                return;

            m_ChargedAmount += amount;
            m_ChargedAmount = Mathf.Clamp(m_ChargedAmount, 0, FULL_CHARGE);
            OnChargeUpdated?.Invoke((int)m_ChargedAmount);

            if (m_ChargedAmount >= FULL_CHARGE)
            {
                ChangeState(State.FullyCharged);
                RaiseFullyChargedEvent();
            }
        }

        public abstract bool TryExecute();

        public virtual void OnDestroy()
        {
            m_CancellationTokenSource.Cancel();
            OnUltimateActionAssigned = null;
            OnChargeUpdated = null;
        }

        protected virtual void Start()
        {
            m_CancellationTokenSource = new CancellationTokenSource();

            ChangeState(State.Charging);
            Charge(-FULL_CHARGE);

            RaiseUltimateActionAssignedEvent();
        }

        protected async void RaiseUltimateActionAssignedEvent()
        {
            // wait for 1 frame to ensure that the event is subscribed to
            await Task.Yield();
            OnUltimateActionAssigned?.Invoke(Name);
        }

        protected abstract void RaiseFullyChargedEvent();

        protected void RaiseUltimateActionExecutedEvent()
        {
            OnUltimateActionExecuted?.Invoke();
        }

        protected async Task ResetAfterDuration(float duration, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay((int)(duration * 1000), cancellationToken);
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
                ChangeState(State.Charging);

                while (CurrentState == State.Charging)
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
