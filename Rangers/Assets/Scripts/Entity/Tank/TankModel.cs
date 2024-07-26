using BTG.Utilities;
using UnityEngine;

namespace BTG.Entity.Tank
{
    public class TankModel : IEntityModel
    {
        public enum TankState
        {
            Idle,
            Moving,
            Deactive
        }

        public bool IsPlayer { get; set; }
        public bool IsNetworkPlayer { get; set; }
        public ulong OwnerClientId { get; set; }
        public ulong NetworkObjectId { get; set; }

        private TankDataSO m_TankData;
        public TankDataSO TankData => m_TankData;

        public TankState State;

        public int MaxHealth => m_TankData.MaxHealth;
        public float Acceleration => m_TankData.Acceleration;
        public int MaxSpeed => m_TankData.MaxSpeed;
        public int RotateSpeed => m_TankData.RotateSpeed;

        public string Name => m_TankData.name;

        public Sprite Icon => m_TankData.Icon;

        /// <summary>
        /// Layer mask of the opposition party to do damage 
        /// (Enemy's opposition is Player and vice-versa)
        /// </summary>
        public int OppositionLayer;

        public TagSO UltimateTag => m_TankData.UltimateTag;

        public TankModel(TankDataSO m_TankData)
        {
            this.m_TankData = m_TankData;
        }

        public void Reset()
        {
            IsPlayer = false;
            IsNetworkPlayer = false;
            NetworkObjectId = default(ulong);
            OppositionLayer = 0;
            State = TankState.Deactive;
        }
    }
}
