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

        /// <summary>
        /// Enable when tank is alive, disable when tank is dead
        /// </summary>
        public bool IsEnabled;
    }
}