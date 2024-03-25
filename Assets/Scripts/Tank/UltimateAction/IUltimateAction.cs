

namespace BTG.Tank.UltimateAction
{
    public interface ICameraShakeUltimateAction
    {
        public event System.Action<float> OnExecuteCameraShake;
    }

    public interface IUltimateAction
    {
        public event System.Action<string> OnUltimateActionAssigned;
        public event System.Action<int> OnChargeUpdated;
        public event System.Action OnFullyCharged;
        public event System.Action OnUltimateActionExecuted;

        public string Name { get; }

        public float ChargeRate { get; }

        public bool IsFullyCharged { get; }

        public void AutoCharge();

        public void Charge(float amount);

        public bool TryExecute();

        public void OnDestroy();
    }
}
