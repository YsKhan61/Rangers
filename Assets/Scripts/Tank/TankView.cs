using UnityEngine;

public class TankView : MonoBehaviour
{
    [SerializeField]
    Rigidbody m_Rigidbody;
    public Rigidbody RigidBody => m_Rigidbody;

    [SerializeField]
    Transform m_FirePoint;
    public Transform FirePoint => m_FirePoint;

    private TankController m_TankController;

    public void SetController(TankController controller)
    {
        m_TankController = controller;
    }

    private void Update()
    {
        m_TankController.Update();
    }
}
