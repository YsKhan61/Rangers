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
        Collider m_Collider;

        [SerializeField]
        Transform m_FirePoint;
        public Transform FirePoint => m_FirePoint;

        [SerializeField]
        TankUI m_TankUI;

        [SerializeField]
        TankAudio m_TankAudio;
        public TankAudio TankAudio => m_TankAudio;

        // dependencies
        private TankBrain m_Controller;
        public TankBrain Controller => m_Controller;

        public Transform Transform => transform;


        public void SetController(TankBrain controller)
        {
            m_Controller = controller;
        }

        public void TakeDamage(int damage)
        {
            m_Controller.HealthController.TakeDamage(damage);
        }

        public void UpdateChargedAmountUI(float chargeAmount)
        {
            m_TankUI.UpdateChargedAmountUI(chargeAmount);
        }

        public void ToggleVisible(bool value)
        {
            m_Graphics.gameObject.SetActive(value);
            m_Collider.enabled = value;
            m_TankAudio.ToggleMuteEngineAudio(!value);
            m_TankAudio.ToggleMuteShootingAudio(!value);
        }
    }
}

