using BTG.Tank;

namespace BTG.Player
{
    public class PlayerModel
    {
        private PlayerDataSO m_PlayerData;
        public PlayerDataSO PlayerData => m_PlayerData;

        public PlayerModel(PlayerDataSO data)
        {
            m_PlayerData = data;
        }

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