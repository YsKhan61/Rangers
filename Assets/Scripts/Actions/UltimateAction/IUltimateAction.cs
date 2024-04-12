
using BTG.Utilities;

namespace BTG.Actions.UltimateAction
{

    public interface IUltimateAction : IDestroyable
    {
        public enum State
        {
            Disabled,
            Charging,
            FullyCharged,
            Executing
        }

        public event System.Action<string> OnUltimateActionAssigned;
        public event System.Action<int> OnChargeUpdated;
        public event System.Action OnFullyCharged;
        public event System.Action OnUltimateActionExecuted;

        public string Name { get; }

        public IUltimateActor Actor { get; } 

        public void Enable();

        public void Disable();

        public void ChangeState(State newState);

        public void AutoCharge();

        public void Charge(float amount);

        public bool TryExecute();
    }
}
