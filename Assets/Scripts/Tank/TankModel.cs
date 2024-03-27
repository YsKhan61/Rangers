using UnityEngine;

namespace BTG.Tank
{
    public class TankModel
    {
        public bool IsPlayer = false;

        private TankDataSO m_TankData;
        public TankDataSO TankData => m_TankData;

        private TankMainController m_TankController;

        public TankMainController.TankState State;

        public float MoveInputValue;
        public float RotateInputValue;

        public float CurrentMoveSpeed => m_TankController.Rigidbody.velocity.magnitude;
        public bool IsCharging;

        public float ChargeAmount;

        private int m_CurrentHealth;
        public int CurrentHealth => m_CurrentHealth;
        public string Name => m_TankData.name;

        public TankModel(TankDataSO m_TankData, TankMainController controller)
        {
            this.m_TankData = m_TankData;
            m_TankController = controller;
        }

        public void AddHealthData(int health)
        {
            m_CurrentHealth += health;
            Mathf.Clamp(m_CurrentHealth, 0, m_TankData.MaxHealth);
        }
    }
}
