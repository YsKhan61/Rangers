using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using State = BTG.Tank.UltimateAction.IUltimateAction.State;
using BTG.Utilities;

namespace BTG.Tank.UltimateAction
{
    public abstract class UltimateAction : IUltimateAction
    {
        public const int FULL_CHARGE = 100;

        public event System.Action<string> OnUltimateActionAssigned;
        public abstract event System.Action OnFullyCharged;
        public event System.Action<int> OnChargeUpdated;
        public event System.Action OnUltimateActionExecuted;

        protected TankUltimateController m_UltimateController;
        protected UltimateActionDataSO m_UltimateActionData;
        protected CancellationTokenSource m_CancellationTokenSource;

        private float m_ChargedAmount;

        public string Name => m_UltimateActionData.name;
        public State CurrentState { get; protected set; }


        public virtual void Enable()
        {
            m_CancellationTokenSource = new CancellationTokenSource();

            ChangeState(State.Charging);
            Charge(-FULL_CHARGE);

            _ = RaiseActionAssignedEventAndStartAutoChargeAsync();
        }

        public virtual void Disable()
        {
            m_CancellationTokenSource.Cancel();
            m_CancellationTokenSource.Dispose();
            OnUltimateActionAssigned = null;
            OnChargeUpdated = null;

            ChangeState(State.Disabled);
        }

        public void ChangeState(State newState)
        {
            CurrentState = newState;
        }

        public void AutoCharge()
        {
            _ = AutoChargeAsync();
        }

        public virtual void Charge(float amount)
        {
            if (CurrentState != State.Charging)
            {
                Debug.LogError("This should not happen!");
                return;
            }

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
            if (m_CancellationTokenSource != null)
            {
                m_CancellationTokenSource.Cancel();
                m_CancellationTokenSource.Dispose();
            }
            
            OnUltimateActionAssigned = null;
            OnChargeUpdated = null;
        }

        protected void RaiseUltimateActionAssignedEvent()
            => OnUltimateActionAssigned?.Invoke(Name);

        protected abstract void RaiseFullyChargedEvent();

        protected void RaiseUltimateActionExecutedEvent()
            => OnUltimateActionExecuted?.Invoke();

        protected void RestartAfterDuration(int duration)
            => _ = HelperMethods.InvokeAfterAsync(duration, () => Restart(), m_CancellationTokenSource.Token);


        protected abstract void Restart();

        private async Task RaiseActionAssignedEventAndStartAutoChargeAsync()
        {
            // wait for 1 frame to ensure that the event is subscribed to
            await Task.Yield();
            RaiseUltimateActionAssignedEvent();

            await Task.Yield();
            AutoCharge();
        }

        private async Task AutoChargeAsync()
        {
            try
            {
                while (CurrentState == State.Charging)
                {
                    await Task.Delay(1000, m_CancellationTokenSource.Token);
                    Charge(m_UltimateActionData.ChargeRate);
                }
            }
            catch (TaskCanceledException)
            {
                // Task was cancelled
            }
        }
    }
}
