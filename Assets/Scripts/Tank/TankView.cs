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
        TankAudioView m_TankAudio;
        public TankAudioView AudioView => m_TankAudio;

        // dependencies
        private TankBrain m_Brain;
        public TankBrain Controller => m_Brain;

        public Transform Transform => transform;


        public void SetController(TankBrain brain)
        {
            m_Brain = brain;
        }

        public void TakeDamage(int damage)
        {
            m_Brain.TakeDamage(damage);
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

