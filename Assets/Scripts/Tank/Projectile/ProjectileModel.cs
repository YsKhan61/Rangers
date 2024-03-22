namespace BTG.Tank.Projectile
{
    public class ProjectileModel
    {
        private ProjectileDataSO m_Data;
        public ProjectileDataSO Data => m_Data;

        private ProjectileController m_Controller;

        public ProjectileModel(ProjectileDataSO data, ProjectileController controller)
        {
            m_Data = data;
            m_Controller = controller;
        }
    }
}

