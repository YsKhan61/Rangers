using BTG.Utilities;
using Unity.Netcode;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkTeslaBallView : NetworkBehaviour, IFiringView
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

        /*[Inject]
        private AudioPool m_AudioPool;*/

        private NetworkTeslaBallPool m_Pool;
        private NetworkTeslaFiring m_TeslaFiring;
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
        public void SetTeslaFiring(NetworkTeslaFiring teslaFiring) => m_TeslaFiring = teslaFiring;

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

            Show_ClientRpc();
        }

        [ClientRpc]
        private void Show_ClientRpc()
        {
            m_ParticleSytem.Play();
            
            if (IsServer)
                m_Collider.enabled = true;
        }

        private void Hide()
        {
            if (!IsServer) return;

            Hide_ClientRpc();
        }

        [ClientRpc]
        private void Hide_ClientRpc()
        {
            m_ParticleSytem.Stop();
            
            if (IsServer)
                m_Collider.enabled = false;
        }

        private void OnHitSomething(Collider other)
        {
            Debug.Log("Hit something: " + other.gameObject.name);

            if (other.TryGetComponent(out IDamageableView damageable))
            {
                damageable.Damage(m_Damage);
            }

            // DoExplosionEffect();
            // DoExplosionAudio;
            Reset();
        }

        private void DoExplosionAudio()
        {
            // m_AudioPool.GetAudioView().PlayOneShot(m_TeslaFiring.Data.ActionImpactClip, transform.position);
        }

        private void Reset()
        {
            m_Rigidbody.Sleep();
            m_TeslaFiring = null;

            Hide();
            m_Pool.ReturnTeslaBall(this);
        }
    }

}

