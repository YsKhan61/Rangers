
namespace BTG.Tank.UltimateAction
{
    public interface IUltimateAction
    {
        public event System.Action<string> OnUltimateActionAssigned;
        public event System.Action<float> OnUltimateExecuted;
        public event System.Action<int> OnChargeUpdated;
        public event System.Action OnFullyCharged;

        public string Name { get; }
        public float Duration { get; }

        public float ChargeRate { get; }

        public bool IsFullyCharged { get; }

        public void Charge(float amount);

        public bool TryExecute(TankUltimateController controller);

        public void OnDestroy();
    }
}
