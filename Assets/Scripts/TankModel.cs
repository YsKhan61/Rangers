public class TankModel
{
    private TankDataSO m_TankData;
    private TankController m_TankController;

    public TankModel(TankDataSO m_TankData, TankController controller)
    {
        this.m_TankData = m_TankData;
        m_TankController = controller;
    }
}
