using UnityEngine;

public class TankView : MonoBehaviour
{
    [SerializeField]
    Rigidbody m_Rigidbody;
    public Rigidbody RigidBody => m_Rigidbody;

    private TankController m_TankController;

    public void SetController(TankController controller)
    {
        m_TankController = controller;
    }

    private void Update()
    {
        m_TankController.Move();
    }
}
