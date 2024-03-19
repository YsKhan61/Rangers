using UnityEngine;

public class TankView : MonoBehaviour
{
    [SerializeField]
    Rigidbody m_Rigidbody;

    private TankController m_TankController;

    public void SetController(TankController controller)
    {
        m_TankController = controller;
    }

    private void FixedUpdate()
    {
        m_Rigidbody.angularVelocity += m_TankController.AngularVelocity * Time.fixedDeltaTime;
        m_Rigidbody.velocity += m_TankController.MoveVelocity * Time.fixedDeltaTime;
    }
}
