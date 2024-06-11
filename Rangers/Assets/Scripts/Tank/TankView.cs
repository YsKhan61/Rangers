using BTG.Entity;
using UnityEngine;


namespace BTG.Tank
{
    public class TankView : MonoBehaviour, IEntityView
    {
        [SerializeField]
        Transform m_CameraTarget;
        public Transform CameraTarget => m_CameraTarget;

        [SerializeField]
        Transform m_Graphics;

        [SerializeField]
        Transform m_FirePoint;
        public Transform FirePoint => m_FirePoint;

        [SerializeField]
        TankUI m_TankUI;

        [SerializeField]
        TankAudioView m_TankAudio;
        public TankAudioView AudioView => m_TankAudio;

        [SerializeField]
        private Collider m_DamageCollider;
        public Collider DamageCollider => m_DamageCollider;

        // dependencies
        private TankBrain m_Brain;
        public TankBrain TankBrain => m_Brain;

        public Transform Transform => transform;


        public void SetBrain(TankBrain brain)
        {
            m_Brain = brain;
        }


        public void UpdateChargedAmountUI(float chargeAmount)
            => m_TankUI.UpdateChargedAmountUI(chargeAmount);

        /// <summary>
        /// Make the tank visible or invisible
        /// </summary>
        /// <param name="show">true - visible</param>
        public void ToggleVisible(bool show)
        {
            m_Graphics.gameObject.SetActive(show);
            
        }

        /// <summary>
        /// Mute/Unmute the audio of the tank
        /// </summary>
        /// <param name="mute">yes - mute</param>
        public void ToggleMuteAudio(bool mute)
        {
            m_TankAudio.ToggleMuteEngineAudio(mute);
            m_TankAudio.ToggleMuteShootingAudio(mute);
        }
    }
}

