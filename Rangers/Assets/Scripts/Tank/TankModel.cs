using BTG.Entity;
using BTG.Utilities;
using UnityEngine;

namespace BTG.Tank
{
    public class TankModel : IEntityTankModel
    {
        public bool IsPlayer { get; set; }

        private TankDataSO m_TankData;
        public TankDataSO TankData => m_TankData;
        

        private TankBrain m_Brain;

        public TankBrain.TankState State;

        public float CurrentMoveSpeed => m_Brain.Rigidbody.velocity.magnitude;

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

        public void SetBrain(TankBrain brain)
        {
            m_Brain = brain;
        }

        public void Reset()
        {
            IsPlayer = false;
            OppositionLayer = 0;
        }
    }
}
