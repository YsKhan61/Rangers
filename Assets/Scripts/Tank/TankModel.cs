public class TankModel
{
    private TankDataSO m_TankData;
    public TankDataSO TankData => m_TankData;

    private TankController m_TankController;

    public bool IsFiring;
    public float LastFireTime;

    public TankModel(TankDataSO m_TankData, TankController controller)
    {
        this.m_TankData = m_TankData;
        m_TankController = controller;
    }
}
