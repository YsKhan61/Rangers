
namespace BTG.Tank.UltimateAction
{
    public interface IUltimateAction
    {
        public const int FULL_CHARGE = 100;

        public string Name { get; }
        public float Duration { get; }

        public float ChargeRate { get; }

        public bool IsFullyCharged { get; }

        public void Charge(float amount);

        public bool TryExecute(TankUltimateController controller);

        public void OnDestroy();
    }
}
