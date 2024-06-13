using BTG.AudioSystem;
using BTG.Utilities;
using UnityEngine;
using VContainer;


namespace BTG.Actions.PrimaryAction
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class TeslaBallView : MonoBehaviour, IFiringView
    {
        public Transform Owner { get; private set; }

        [SerializeField]
        Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        [SerializeField]
        ParticleSystem m_ParticleSytem;

        [SerializeField]
        SphereCollider m_Collider;
        public SphereCollider Collider => m_Collider;

        [Inject]
        private AudioPool m_AudioPool;

        private TeslaFiring m_TeslaFiring;
        private TeslaBallPool m_Pool;

        private int m_Damage;


        private void OnCollisionEnter(Collision collision) => OnHitSomething(collision.collider);

        private void OnTriggerEnter(Collider other) => OnHitSomething(other);

        private void OnHitSomething(Collider other)
        {
            if (other.TryGetComponent(out IDamageableView damageable))
            {
                damageable.Damage(m_Damage);
            }

            DoExplosionAudio();
            Reset();
        }

        /// <summary>
        /// Set the owner of the tesla ball
        /// </summary>
        public void SetOwner(Transform owner) => Owner = owner;

        /// <summary>
        /// Set the tesla firing that fired the tesla ball
        /// </summary>
        public void SetTeslaFiring(TeslaFiring teslaFiring) => m_TeslaFiring = teslaFiring;

        /// <summary>
        /// Add impulse force to the tesla ball to move it forward
        /// </summary>
        public void AddImpulseForce(float force)
            => m_Rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);

        /// <summary>
        /// Set the pool to return the tesla ball to
        /// </summary>
        public void SetPool(TeslaBallPool pool) => m_Pool = pool;

        /// <summary>
        /// Set the damage the tesla ball will do
        /// </summary>
        public void SetDamage(int damage) => m_Damage = damage;

        private void DoExplosionAudio()
        {
            m_AudioPool.GetAudioView().PlayOneShot(m_TeslaFiring.Data.ActionImpactClip, transform.position);
        }

        private void Reset()
        {
            m_Rigidbody.Sleep();
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            m_TeslaFiring = null;

            Hide();

            m_Pool.ReturnTeslaBall(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            m_ParticleSytem.Play();
            m_Collider.enabled = true;
        }

        private void Hide()
        {
            m_ParticleSytem.Stop();
            gameObject.SetActive(false);
            m_Collider.enabled = false;
        }

#if UNITY_EDITOR

        private void DebugCollision(string colliderName)
            => Debug.Log("Tesla ball collided with " + colliderName);

#endif
    }
}

