using BTG.Utilities;

namespace BTG.Tank
{
    public class TankPool : GenericObjectPool<TankController>
    {
        public TankController GetTank()
        {
            TankController tank = GetItem();
            // tank.Init();
            return tank;
        }

        public void ReturnTank(TankController tank)
        {
            ReturnItem(tank);
        }

        protected override TankController CreateItem() => null;//new TankController(this);
    }
}