using UnityEngine;

public class TankView : MonoBehaviour
{
    [SerializeField]
    Transform m_CameraTarget;
    public Transform CameraTarget => m_CameraTarget;

    [SerializeField]
    Rigidbody m_Rigidbody;
    public Rigidbody RigidBody => m_Rigidbody;

    [SerializeField]
    Transform m_FirePoint;
    public Transform FirePoint => m_FirePoint;

    [SerializeField]
    TankUI m_TankUI;

    // dependencies
    private TankController m_TankController;

    public void SetController(TankController controller)
    {
        m_TankController = controller;
    }

    public void UpdateChargedAmountUI(float chargeAmount)
    {
        m_TankUI.UpdateChargedAmountUI(chargeAmount);
    }

    private void Update()
    {
        m_TankController.Update();
    }
}
