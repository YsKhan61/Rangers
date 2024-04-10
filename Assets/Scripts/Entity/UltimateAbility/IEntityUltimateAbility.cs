

namespace BTG.Entity
{

    public interface IEntityUltimateAbility
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

        public IEntityUltimateController Controller { get; } 

        public void Enable();

        public void Disable();

        public void ChangeState(State newState);

        public void AutoCharge();

        public void Charge(float amount);

        public bool TryExecute();

        public void OnDestroy();
    }
}
