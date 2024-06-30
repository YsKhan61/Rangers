using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    [RequireComponent(typeof(NetworkTransform))]
    public class NetworkProjectileView : NetworkBehaviour, IProjectileView
    {
        [SerializeField]
        GameObject[] m_Graphics;

        [SerializeField]
        Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        [SerializeField] 
        Collider m_Collider;

        [SerializeField]
        NetworkTransform m_NetworkTransform;

        public Transform Owner { get; private set; }
        public Transform Transform => transform;

        private ProjectileController m_Controller;
        private NetworkProjectilePool m_Pool;



        /// <summary>
        /// This is for environment objects
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if (!IsServer) return;

            m_Controller?.OnHitSomething(collision.collider);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer) return;

            m_Controller?.OnHitSomething(other);
        }

        public void SetController(ProjectileController controller) => m_Controller = controller;
        public void SetPool(NetworkProjectilePool pool) => m_Pool = pool;
        public void SetOwner(Transform owner) => Owner = owner;
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            if (!IsServer) return;
            m_NetworkTransform.Teleport(position, rotation, Vector3.one);
        }

        public void ReturnToPool()
        {
            if (!IsServer) return;

            m_Pool.ReturnProjectile(this);
        }

        public void Show()
        {
            if (!IsServer) return;
            StartCoroutine(StartWithDelay());
        }

        IEnumerator StartWithDelay()
        {
            yield return new WaitForSeconds(0.1f);
            Show_ClientRpc();
        }

        [ClientRpc]
        private void Show_ClientRpc()
        {
            ToggleGraphics(true);

            if (IsServer)
                m_Collider.enabled = true;
        }

        public void Hide()
        {
            if (!IsServer) return;
            Hide_ClientRpc();
        }

        [ClientRpc]
        private void Hide_ClientRpc()
        {
            ToggleGraphics(false);

            if (IsServer)
                m_Collider.enabled = false;
        }

        private void ToggleGraphics(bool show)
        {
            if (m_Graphics == null)
                return;

            foreach (var item in m_Graphics)
            {
                item.SetActive(show);
            }
        }
    }
}

