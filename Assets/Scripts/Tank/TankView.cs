using BTG.Utilities;
using UnityEngine;

namespace BTG.Tank
{
    public class TankView : MonoBehaviour, IDamageable
    {
        [SerializeField]
        Transform m_CameraTarget;
        public Transform CameraTarget => m_CameraTarget;

        [SerializeField]
        Transform m_Graphics;

        [SerializeField]
        Rigidbody m_Rigidbody;
        public Rigidbody RigidBody => m_Rigidbody;

        [SerializeField]
        Transform m_FirePoint;
        public Transform FirePoint => m_FirePoint;

        [SerializeField]
        TankUI m_TankUI;

        [SerializeField]
        TankAudio m_TankAudio;
        public TankAudio TankAudio => m_TankAudio;

        // dependencies
        private TankController m_Controller;
        public TankController Controller => m_Controller;


        private void FixedUpdate()
        {
            m_Controller.FixedUpdate();
        }

        private void Update()
        {
            m_Controller.Update();
        }

        private void OnDestroy()
        {
            m_Controller.OnDestroy();
        }

        public void SetController(TankController controller)
        {
            m_Controller = controller;
        }

        public void TakeDamage(int damage)
        {
            m_Controller.TakeDamage(damage);
        }

        public void UpdateChargedAmountUI(float chargeAmount)
        {
            m_TankUI.UpdateChargedAmountUI(chargeAmount);
        }

        public void Show()
        {
            m_Graphics.gameObject.SetActive(true);
        }

        public void Hide()
        {
            m_Graphics.gameObject.SetActive(false);
        }
    }
}

