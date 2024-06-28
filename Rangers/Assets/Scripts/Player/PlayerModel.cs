using UnityEngine;


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

        /// <summary>
        /// Enable when tank is alive, disable when tank is dead
        /// </summary>
        public bool IsAlive;

        // caches
        public int EntityMaxSpeed;
        public int EntityRotateSpeed;
        public float EntityAcceleration;
        public float Acceleration;       // entity acceleration multiplied by move input value
        public float RotateAngle;
        public Quaternion DeltaRotation;
    }
}