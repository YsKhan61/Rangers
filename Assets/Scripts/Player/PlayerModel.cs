using BTG.Entity;

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

        public IEntityModel EntityModel;

        public int MaxHealth => EntityModel.MaxHealth;

        public int EntityMaxSpeed => EntityModel.MaxSpeed;
        public int EntityRotateSpeed => EntityModel.RotateSpeed;
        public float EntityAcceleration => EntityModel.Acceleration;

        /// <summary>
        /// Enable when tank is alive, disable when tank is dead
        /// </summary>
        public bool IsEnabled;
    }
}