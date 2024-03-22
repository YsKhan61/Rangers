
namespace BTG.Tank.UltimateAction
{
    public interface IUltimateAction
    {
        public float Duration { get; }

        public void Execute(TankUltimateController controller);

        public void OnDestroy();
    }
}
