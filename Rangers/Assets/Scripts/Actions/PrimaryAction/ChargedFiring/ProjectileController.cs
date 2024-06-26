using BTG.AudioSystem;
using BTG.Effects;
using BTG.Utilities;
using System.Threading;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    public class ProjectileController : IDestroyable
    {
        private AudioPool m_AudioPool;
        private ChargedFiringDataSO m_Data;
        private IProjectileView m_View;
        private CancellationTokenSource m_Cts;
        public Transform Transform { get; private set; }

        public ProjectileController(ChargedFiringDataSO projectileData, IProjectileView view)
        {
            m_Cts = new CancellationTokenSource();
            m_Data = projectileData;
            m_View = view;

            Transform = m_View.Transform;
        }

        public void Init()
        {
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);
            m_View.Rigidbody.isKinematic = false;
        }

        public void SetAudioPool(AudioPool audioPool) => m_AudioPool = audioPool;

        public void ShowView() => m_View.Show();

        public void SetOwnerOfView(Transform owner) => m_View.SetOwner(owner);
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation) => 
            m_View.SetPositionAndRotation(position, rotation);

        public void Destroy()
        {
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_Cts);
        }

        public void AddImpulseForce(float initialSpeed)
        {
            m_View.Rigidbody.AddForce(Transform.forward * initialSpeed, ForceMode.Impulse);
        }

        public void OnHitSomething(Collider other)
        {
            if (other.TryGetComponent(out IDamageableView damageable))
            {
                if (damageable.Owner == m_View.Owner)
                {
                    return;
                }
                damageable.Damage(m_Data.Damage);
            }

            // DoExplosionAudio();
            ResetProjectile();
        }

        /*private void DoExplosionAudio()
        {
            m_AudioPool.GetAudioView().PlayOneShot(m_Data.ActionImpactClip, Transform.position);
        }*/

        private void ResetProjectile()
        {
            m_View.Rigidbody.velocity = Vector3.zero;
            m_View.Rigidbody.angularVelocity = Vector3.zero;
            m_View.Rigidbody.isKinematic = true;

            m_View.Hide();
            m_View.ReturnToPool();

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }
    }
}

