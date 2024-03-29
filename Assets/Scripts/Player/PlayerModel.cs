using BTG.Tank;

namespace BTG.Player
{
    public class PlayerModel
    {
        public float MoveInputValue;
        public float RotateInputValue;

        public TankModel TankModel;

        public int TankMaxSpeed => TankModel.TankData.MaxSpeed;
        public int TankRotateSpeed => TankModel.TankData.RotateSpeed;
        public float TankAcceleration => TankModel.TankData.Acceleration;
    }
}