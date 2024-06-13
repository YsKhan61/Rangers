using BTG.Factory;
using BTG.Utilities;
using System.Threading;
using UnityEngine;

namespace BTG.Effects
{
    /// <summary>
    /// The view of the ragdoll.
    /// It is responsible for the visual representation of the ragdoll
    /// It can be created using the factory as it implements the IFactoryItem interface
    /// It has a reference to the pool to return itself after the effect is done
    /// </summary>
    public class RagdollView : MonoBehaviour, IFactoryItem
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
        /// Execute the ragdoll effect with the owner
        /// </summary>
        public void ExecuteRagdollEffect(IRagdollOwner owner)
        {
            SetOwner(owner);
            Execute(new Pose(owner.Transform.position, owner.Transform.rotation));
        }

        /// <summary>
        /// This method is called when the ragdoll is created
        /// It is called by the pool only one time
        /// </summary>
        internal void Initialize()
        {
            m_Cts = new CancellationTokenSource();
            StoreInitialLocalPoses();
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
            ResetLocalPoses();
            m_Pool.ReturnRagdoll(this);
        }

        private void StoreInitialLocalPoses()
        {
            m_InitialPoses = new Pose[m_Rigidbodies.Length];
            for (int i = 0; i < m_Rigidbodies.Length; i++)
            {
                m_InitialPoses[i] = new Pose(m_Rigidbodies[i].transform.localPosition, m_Rigidbodies[i].transform.localRotation);
            }
        }

        private void ResetLocalPoses()
        {
            for (int i = 0; i < m_Rigidbodies.Length; i++)
            {
                m_Rigidbodies[i].transform.localPosition = m_InitialPoses[i].position;
                m_Rigidbodies[i].transform.localRotation = m_InitialPoses[i].rotation;
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