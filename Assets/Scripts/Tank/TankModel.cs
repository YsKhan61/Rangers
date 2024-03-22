namespace BTG.Tank
{
    public class TankModel
    {
        private TankDataSO m_TankData;
        public TankDataSO TankData => m_TankData;

        private TankController m_TankController;

        public TankController.TankState State;

        public float MoveInputValue;
        public float RotateInputValue;

        public float CurrentMoveSpeed => m_TankController.Rigidbody.velocity.magnitude;
        public bool IsCharging;

        public float ChargeAmount;

        public TankModel(TankDataSO m_TankData, TankController controller)
        {
            this.m_TankData = m_TankData;
            m_TankController = controller;
        }
    }
}
