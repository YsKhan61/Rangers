using BTG.Utilities;
using UnityEngine;


namespace BTG.Tank.Projectile
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        [SerializeField] private Collider m_Collider;

        [SerializeField] private ParticleSystem m_ExplosionParticle;
        [SerializeField] private AudioSource m_ExplosionAudioSource;

        private ProjectileController m_Controller;

        private void OnEnable()
        {
            m_Collider.enabled = true;
        }

        public void OnCollisionEnter(Collision other)
        {
            if (other.collider.TryGetComponent(out IDamageable damageable))
            {
                m_Controller.OnHitDamageable(damageable);
            }
            else
            {
                m_Controller.ResetProjectile();
            }

            m_Collider.enabled = false;
        }

        private void OnDisable()
        {
            m_Collider.enabled = false;
        }

        private void OnDestroy()
        {
            m_Controller.OnDestroy();
        }

        public void SetController(ProjectileController controller)
        {
            m_Controller = controller;
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

