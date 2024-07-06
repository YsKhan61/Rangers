using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class TeslaBallView : MonoBehaviour, ITeslaBallView
    {
        public Transform Transform => transform;

        public Transform Owner { get; private set; }

        [SerializeField]
        Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        [SerializeField]
        ParticleSystem m_ParticleSytem;

        [SerializeField]
        SphereCollider m_Collider;
        public SphereCollider Collider => m_Collider;

        private TeslaFiringBase m_TeslaFiring;
        private TeslaFiringDataSO m_Data;
        private TeslaBallPool m_Pool;
        private int m_Damage;


        private void OnCollisionEnter(Collision collision) => OnHitSomething(collision.collider);

        private void OnTriggerEnter(Collider other) => OnHitSomething(other);

        /// <summary>
        /// Set the owner of the tesla ball
        /// </summary>
        public void SetOwner(Transform owner) => Owner = owner;

        /// <summary>
        /// Set the tesla firing that fired the tesla ball
        /// </summary>
        public void SetTeslaFiring(TeslaFiringBase teslaFiring)
        {
            m_TeslaFiring = teslaFiring;
            m_Data = m_TeslaFiring.Data;
        }

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

        public virtual void Show()
        {
            gameObject.SetActive(true);
            m_ParticleSytem.Play();
            m_Collider.enabled = true;
        }

        protected virtual void Hide()
        {
            m_ParticleSytem.Stop();
            gameObject.SetActive(false);
            m_Collider.enabled = false;
        }

        private void OnHitSomething(Collider other)
        {
            // NOTE - This need to happen before the damage is done, as we need the data of the TeslaFiring to do the effect
            // If the damageable.Damage is called first, there is a chance that the entity is destroyed and the data is lost
            DoExplosionEffect();

            if (other.TryGetComponent(out IDamageableView damageable))
            {
                damageable.Damage(m_Damage);
            }

            Reset();
        }

        private void DoExplosionEffect()
        {
            EventBus<EffectEventData>.Invoke(new EffectEventData
            {
                Tag = m_TeslaFiring.Data.HitEffectTag,
                Position = transform.position
            });
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

        

#if UNITY_EDITOR

        private void DebugCollision(string colliderName)
            => Debug.Log("Tesla ball collided with " + colliderName);

#endif
    }

}

