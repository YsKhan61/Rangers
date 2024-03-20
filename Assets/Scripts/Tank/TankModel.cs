public class TankModel
{
    private TankDataSO m_TankData;
    public TankDataSO TankData => m_TankData;

    private TankController m_TankController;

    private bool m_IsFiring;
    public bool IsFiring => m_IsFiring;

    public TankModel(TankDataSO m_TankData, TankController controller)
    {
        this.m_TankData = m_TankData;
        m_TankController = controller;
    }

    public void ToggleFiring(bool isFiring)
    {
        m_IsFiring = isFiring;
    }
}
