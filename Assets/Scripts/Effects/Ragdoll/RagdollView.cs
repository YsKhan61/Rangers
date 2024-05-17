﻿using BTG.Utilities;
using System.Threading;
using UnityEngine;

namespace BTG.Effects
{
    public class RagdollView : MonoBehaviour
    {
        private RagdollPool m_Pool;
        private IRagdollOwner m_Owner;      // maybe needed for resetting early during revive
        private const int DESTROY_DELAY = 2; // This can be passed from either owner, or the factory

        [SerializeField, Tooltip("The rigidbodies of the ragdoll")]
        private Rigidbody[] m_Rigidbodies;

        private Pose[] m_InitialPoses;

        private CancellationTokenSource m_Cts;

        private void OnDestroy()
        {
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_Cts);
        }

        /// <summary>
        /// This method is called when the ragdoll is created
        /// It is called by the pool only one time
        /// </summary>
        internal void Initialize()
        {
            m_Cts = new CancellationTokenSource();
            StoreInitialPoses();
            Hide();
            ToggleRigidbodyKinematics(true);
        }

        /// <summary>
        /// Set the pool of the ragdoll
        /// </summary>
        internal void SetPool(RagdollPool pool) => m_Pool = pool;

        internal void SetOwner(IRagdollOwner owner) => m_Owner = owner;

        /// <summary>
        /// Execute the ragdoll effect
        /// </summary>
        internal void Execute(in Pose pose)
        {
            transform.position = pose.position;
            transform.rotation = pose.rotation;

            ToggleRigidbodyKinematics(false);
            Show();

            _ = HelperMethods.InvokeAfterAsync(DESTROY_DELAY, () =>
            {
                ResetView();
            }, m_Cts.Token);
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

        private void ResetView()
        {
            Hide();
            ToggleRigidbodyKinematics(true);
            ResetPoses();
            m_Pool.ReturnRagdoll(this);
        }

        private void StoreInitialPoses()
        {
            m_InitialPoses = new Pose[m_Rigidbodies.Length];
            for (int i = 0; i < m_Rigidbodies.Length; i++)
            {
                m_InitialPoses[i] = new Pose(m_Rigidbodies[i].transform.position, m_Rigidbodies[i].transform.rotation);
            }
        }

        private void ResetPoses()
        {
            for (int i = 0; i < m_Rigidbodies.Length; i++)
            {
                m_Rigidbodies[i].transform.position = m_InitialPoses[i].position;
                m_Rigidbodies[i].transform.rotation = m_InitialPoses[i].rotation;
            }
        }

        private void ToggleRigidbodyKinematics(bool isKinematic)
        {
            for (int i = 0; i < m_Rigidbodies.Length; i++)
            {
                m_Rigidbodies[i].isKinematic = isKinematic;
            }
        }
    }
}