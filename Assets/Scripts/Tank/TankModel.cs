using UnityEngine;

namespace BTG.Tank
{
    public class TankModel
    {
        public bool IsPlayer = false;

        private TankDataSO m_TankData;
        public TankDataSO TankData => m_TankData;

        private TankBrain m_TankController;

        public TankBrain.TankState State;

        public float CurrentMoveSpeed => m_TankController.Rigidbody.velocity.magnitude;
        public bool IsCharging;

        public float ChargeAmount;

        private int m_CurrentHealth;
        public int CurrentHealth => m_CurrentHealth;
        public string Name => m_TankData.name;

        /// <summary>
        /// Layer mask of the opposition party to do damage 
        /// (Enemy's opposition is Player and vice-versa)
        /// </summary>
        public int OppositionLayer;

        public TankModel(TankDataSO m_TankData, TankBrain controller)
        {
            this.m_TankData = m_TankData;
            m_TankController = controller;
        }

        public void AddHealthData(int health)
        {
            m_CurrentHealth += health;
            Mathf.Clamp(m_CurrentHealth, 0, m_TankData.MaxHealth);
        }

        public void Reset()
        {
            IsPlayer = false;
            State = TankBrain.TankState.Idle;
            IsCharging = false;
            ChargeAmount = 0;
            m_CurrentHealth = m_TankData.MaxHealth;
            OppositionLayer = 0;
        }
    }
}
