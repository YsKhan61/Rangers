using BTG.Utilities;
using UnityEngine;


namespace BTG.Tank.Projectile
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        [SerializeField] private ParticleSystem m_ExplosionParticle;
        [SerializeField] private AudioSource m_ExplosionAudioSource;

        private ProjectileController m_Controller;

        public void SetController(ProjectileController controller)
        {
            m_Controller = controller;
        }

        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                m_Controller.OnHitDamageable(damageable);
                return;
            }

            m_Controller.ResetProjectile();
        }

        private void OnDestroy()
        {
            m_Controller.OnDestroy();
        }

        public void PlayExplosionSound(AudioClip clip)
        {
            m_ExplosionAudioSource.PlayOneShot(clip);
        }

        public void PlayExplosionParticle()
        {
            m_ExplosionParticle.Play();
        }

        public float ExplosionDuration => m_ExplosionParticle.main.duration;
    }
}

