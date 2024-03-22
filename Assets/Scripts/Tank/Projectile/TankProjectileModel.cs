namespace BTG.Tank.Projectile
{
    public class TankProjectileModel
    {
        private TankProjectileDataSO m_Data;
        public TankProjectileDataSO Data => m_Data;

        private TankProjectileController m_Controller;

        public TankProjectileModel(TankProjectileDataSO data, TankProjectileController controller)
        {
            m_Data = data;
            m_Controller = controller;
        }
    }
}

