using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using BTG.Utilities;
using State = BTG.Actions.UltimateAction.IUltimateAction.State;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// The base class for the some ultimate actions.
    /// It contains the common properties and methods of the ultimate actions.
    /// </summary>
    public abstract class BaseUltimateAction : IUltimateAction
    {
        public const int FULL_CHARGE = 100;

        public event System.Action<TagSO> OnUltimateActionAssigned;
        public abstract event System.Action OnFullyCharged;
        public event System.Action<int> OnChargeUpdated;
        public event System.Action OnUltimateActionExecuted;        

        public TagSO Tag => m_UltimateActionData.Tag;
        public State CurrentState { get; protected set; }

        public IUltimateActor Actor { get; protected set; }

        protected UltimateActionDataSO m_UltimateActionData;
        protected CancellationTokenSource m_CTS;

        private float m_ChargedAmount;

        public virtual void Enable()
        {
            m_CTS = new();

            ChangeState(State.Charging);
            Charge(-FULL_CHARGE);

            _ = RaiseActionAssignedEventAndStartAutoChargeAsync();

            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);
        }

        public virtual void Disable()
        {
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_CTS);

            Charge(-FULL_CHARGE);

            OnUltimateActionAssigned = null;
            OnChargeUpdated = null;

            ChangeState(State.Disabled);

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }

        public void SetActor(IUltimateActor actor) => Actor = actor;

        protected void ChangeState(State newState)
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
                Debug.Log("Not in charging state:!");
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

        public virtual void Destroy()
        {
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_CTS);
            
            OnUltimateActionAssigned = null;
            OnChargeUpdated = null;
        }

        protected void RaiseUltimateActionAssignedEvent()
            => OnUltimateActionAssigned?.Invoke(Tag);

        protected abstract void RaiseFullyChargedEvent();

        protected void RaiseUltimateActionExecutedEvent()
            => OnUltimateActionExecuted?.Invoke();

        protected void RestartAfterDuration(int duration)
            => _ = HelperMethods.InvokeAfterAsync(duration, () => Restart(), m_CTS.Token);


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
                    await Task.Delay(1000, m_CTS.Token);
                    Charge(m_UltimateActionData.ChargeRate);
                    // Debug.Log("Charging : " + m_ChargedAmount + " : " + Actor.Transform.gameObject.name);
                }
            }
            catch (TaskCanceledException)
            {
                // Task was cancelled
            }
        }
    }
}
