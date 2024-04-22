using BTG.Utilities;
using UnityEngine;

namespace BTG.Tank
{
    public class TankView : MonoBehaviour
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
        private TankDamageableView m_DamageableView;
        public IDamageable Damageable => m_DamageableView;

        // dependencies
        private TankBrain m_Brain;
        public TankBrain TankBrain => m_Brain;

        public Transform Transform => transform;


        public void SetBrain(TankBrain brain)
        {
            m_Brain = brain;
            m_DamageableView.SetBrain(m_Brain);
        }

        public void SetDamageableLayer(int layer) 
            => m_DamageableView.gameObject.layer = layer;


        public void UpdateChargedAmountUI(float chargeAmount)
            => m_TankUI.UpdateChargedAmountUI(chargeAmount);

        public void ToggleVisible(bool isVisible)
        {
            m_Graphics.gameObject.SetActive(isVisible);
            m_DamageableView.ToogleDamageableCollider(isVisible);
            m_TankAudio.ToggleMuteEngineAudio(!isVisible);
            m_TankAudio.ToggleMuteShootingAudio(!isVisible);
        }
    }
}

