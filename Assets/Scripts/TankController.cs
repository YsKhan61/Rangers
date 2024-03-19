using UnityEngine;

public class TankController
{
    private TankModel m_TankModel;
    private TankView m_TankView;

    public Vector3 MoveVelocity => m_TankView.transform.forward * m_TankModel.TankData.MoveSpeed * m_inputControls.Player.MoveAction.ReadValue<float>();
    public Vector3 AngularVelocity => Vector3.up * m_TankModel.TankData.RotateSpeed * m_inputControls.Player.RotateAction.ReadValue<float>();

    private InputControls m_inputControls;

    public TankController(TankDataSO tankData)
    {
        m_TankModel = new TankModel(tankData, this);
        m_TankView = Object.Instantiate(tankData.TankViewPrefab);
        m_TankView.SetController(this);

        ConfigureInputs();
    }

    ~TankController()
    {
        m_inputControls.Player.Disable();
    }

    private void ConfigureInputs()
    {
        m_inputControls = new InputControls();
        m_inputControls.Enable();
        m_inputControls.Player.Enable();
    }
}
