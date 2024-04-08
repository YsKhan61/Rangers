using UnityEngine;

namespace BTG.Tank
{
    public class TankModel
    {
        public bool IsPlayer = false;

        private TankDataSO m_TankData;
        public TankDataSO TankData => m_TankData;

        public float Acceleration => m_TankData.Acceleration;
        public int MaxSpeed => m_TankData.MaxSpeed;

        private TankBrain m_Brain;

        public TankBrain.TankState State;

        public float CurrentMoveSpeed => m_Brain.Rigidbody.velocity.magnitude;
        public bool IsCharging;

        public float ChargeAmount;

        private int m_CurrentHealth;
        public int CurrentHealth => m_CurrentHealth;

        public int MaxHealth => m_TankData.MaxHealth;

        public string Name => m_TankData.name;

        public Sprite Icon => m_TankData.Icon;

        /// <summary>
        /// Layer mask of the opposition party to do damage 
        /// (Enemy's opposition is Player and vice-versa)
        /// </summary>
        public int OppositionLayer;

        public TankModel(TankDataSO m_TankData, TankBrain brain)
        {
            this.m_TankData = m_TankData;
            m_Brain = brain;
        }

        public void AddHealthData(int health)
        {
            m_CurrentHealth += health;
            Mathf.Clamp(m_CurrentHealth, 0, m_TankData.MaxHealth);
        }

        public void Reset()
        {
            IsPlayer = false;
            IsCharging = false;
            ChargeAmount = 0;
            m_CurrentHealth = 0;
            OppositionLayer = 0;
        }
    }
}
