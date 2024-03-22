public class TankUltimateController
{
    private IUltimateAction m_UltimateAction;

    public TankUltimateController(IUltimateAction action)
    {
        m_UltimateAction = action;
    }

    public void ExecuteUltimateAction()
    {
        m_UltimateAction.Execute();
    }
}
