﻿using BTG.Utilities;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    [RequireComponent(typeof(NetworkObject), typeof(Rigidbody), typeof(NetworkRigidbody))]
    public class NetworkProjectileView : NetworkBehaviour, IProjectileView
    {
        [SerializeField]
        GameObject m_Graphics;

        [SerializeField]
        Rigidbody m_Rigidbody;
        public Rigidbody Rigidbody => m_Rigidbody;

        [SerializeField] 
        Collider m_Collider;

        public Transform Owner { get; private set; }
        public Transform Transform => transform;

        private ProjectileController m_Controller;
        private NetworkProjectilePool m_Pool;


        /// <summary>
        /// This is for environment objects
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            m_Controller.OnHitSomething(collision.collider);
            InvokeEffectEvents();
        }

        private void OnTriggerEnter(Collider other)
        {
            m_Controller.OnHitSomething(other);
        }

        public void SetController(ProjectileController controller) => m_Controller = controller;
        public void SetPool(NetworkProjectilePool pool) => m_Pool = pool;
        public void SetOwner(Transform owner) => Owner = owner;

        public void ReturnToPool()
        {
            if (!IsServer) return;

            m_Pool.ReturnProjectile(this);
        }

        public void Show()
        {
            if (!IsServer) return;
            Show_ClientRpc();
        }

        [ClientRpc]
        private void Show_ClientRpc()
        {
            m_Graphics.SetActive(true);

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
            m_Graphics.SetActive(false);

            if (IsServer)
                m_Collider.enabled = false;
        }

        private void InvokeEffectEvents()
        {
            // Invoke the effect events such as explosion effect and audio
        }
    }
}

