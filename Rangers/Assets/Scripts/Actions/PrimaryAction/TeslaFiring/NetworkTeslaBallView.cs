using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using Unity.Netcode;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkTeslaBallView : NetworkBehaviour, ITeslaBallView
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

        private NetworkTeslaBallPool m_Pool;
        private ITeslaFiring m_TeslaFiring;
        private TeslaFiringDataSO m_Data;
        private int m_Damage;
        private TagSO m_EffectTag;


        private void OnCollisionEnter(Collision collision) => OnHitSomething(collision.collider);

        private void OnTriggerEnter(Collider other) => OnHitSomething(other);

        /// <summary>
        /// Set the owner of the tesla ball
        /// </summary>
        public void SetOwner(Transform owner) => Owner = owner;

        /// <summary>
        /// Set the tesla firing that fired the tesla ball
        /// </summary>
        public void SetTeslaFiring(ITeslaFiring teslaFiring)
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
        public void SetPool(NetworkTeslaBallPool pool) => m_Pool = pool;

        /// <summary>
        /// Set the damage the tesla ball will do
        /// </summary>
        public void SetDamage(int damage) => m_Damage = damage;

        public void Show()
        {
            if (!IsServer) return;
            m_Collider.enabled = true;
            Show_ClientRpc();
        }

        [ClientRpc]
        private void Show_ClientRpc()
        {
            m_ParticleSytem.Play();
        }

        private void Hide()
        {
            if (!IsServer) return;
            m_Collider.enabled = false;
            Hide_ClientRpc();
        }

        [ClientRpc]
        private void Hide_ClientRpc()
        {
            m_ParticleSytem.Stop();
        }

        private void OnHitSomething(Collider other)
        {
            if (!IsServer) return;

            // Debug.Log("Hit something: " + other.gameObject.name);

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
            EventBus<NetworkEffectEventData>.Invoke(new NetworkEffectEventData
            {
                OwnerClientOnly = false,
                FollowNetworkObject = false,
                TagNetworkGuid = m_Data.HitEffectTag.Guid.ToNetworkGuid(),
                EffectPosition = transform.position
            });
        }

        private void Reset()
        {
            m_TeslaFiring = null;

            Hide();
            m_Pool.ReturnTeslaBall(this);
        }
    }

}

