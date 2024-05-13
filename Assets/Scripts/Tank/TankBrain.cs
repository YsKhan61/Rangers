using BTG.Actions.PrimaryAction;
using BTG.Actions.UltimateAction;
using BTG.Entity;
using BTG.Utilities;
using System;
using UnityEngine;
using Object = UnityEngine.Object;


namespace BTG.Tank
{
    /// <summary>
    /// The TankBrain for the tank. It handles the communications between Model, View and other controllers such as 
    /// PrimaryAction, UltimateAction.
    /// </summary>
    public class TankBrain : IEntityTankBrain, IUpdatable, IDestroyable
    {
        public event Action<Sprite> OnEntityInitialized;
        public event Action<bool> OnEntityVisibilityToggled;

        public enum TankState
        {
            Idle,
            Moving,
            Dead
        }


        private TankModel m_Model;
        IEntityTankModel IEntityTankBrain.Model => m_Model;
        private TankView m_View;
        private IPrimaryAction m_PrimaryAction;
        public IPrimaryAction PrimaryAction => m_PrimaryAction;
        private IUltimateAction m_UltimateAction;
        public IUltimateAction UltimateAction => m_UltimateAction;
        public Transform Transform => m_View.transform;
        public Transform CameraTarget => m_View.CameraTarget;
        public Rigidbody Rigidbody { get; private set; }
        public Transform FirePoint => m_View.FirePoint;
        public LayerMask OppositionLayerMask => m_Model.OppositionLayer;
        public Collider DamageCollider => m_View.DamageCollider;
        public IDamageable Damageable { get; private set; }

        public bool IsPlayer { get => m_Model.IsPlayer; set => m_Model.IsPlayer = value; }
        public float CurrentMoveSpeed => m_Model.CurrentMoveSpeed;
        
        private TankPool m_Pool;



        /// <summary>
        /// This constructor creates all the respective properties of the tank that are mandatory
        /// for the tank to function properly.
        /// TankView, TankModel, TankMovementController, TankFiringController, TankUltimateController, TankHealthController
        /// </summary>
        /// <param name="tankData"></param>
        /// <param name="pool"></param>
        public TankBrain(TankDataSO tankData, TankPool pool)
        {
            m_Pool = pool;

            m_Model = new TankModel(tankData, this);
            m_View = Object.Instantiate(tankData.TankViewPrefab, m_Pool.TankContainer);
            m_View.SetBrain(this); 

            m_PrimaryAction = m_Model.TankData.PrimaryActionFactory.CreatePrimaryAction(this);
            m_UltimateAction = m_Model.TankData.UltimateActionFactory.CreateUltimateAction(this);
        }

        /// <summary>
        /// Initialize the tank brain.
        /// It sets the tank state to idle, enables the primary and ultimate actions, resets the health controller.
        /// It registers the tank to update and destroy callbacks.
        /// It raises the initialized event.
        /// </summary>
        public void Init()
        {
            m_Model.State = TankState.Idle;

            OnTankStateChangedToIdle();
            ToggleActorVisibility(true);

            m_PrimaryAction.Enable();
            m_UltimateAction.Enable();

            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);

            _ = HelperMethods.InvokeInNextFrame(()=> OnEntityInitialized?.Invoke(m_Model.Icon));
        }

        public void SetRigidbody(Rigidbody rb) => Rigidbody = rb;
        public void SetDamageable(IDamageable damageable) => Damageable = damageable;

        public void Update()
        {
            UpdateState();
            UpdateMoveSound();
        }

        public void Destroy()
        {
            OnEntityInitialized = null;
        }

        public void Die()
        {
            m_Model.State = TankState.Dead;
            OnTankStateChangedToDead();
        }

        public void ToggleActorVisibility(bool value)
        {
            OnEntityVisibilityToggled?.Invoke(value);
            m_View.ToggleVisible(value);
            m_View.ToggleMuteAudio(!value);
        }
        public void SetParentOfView(Transform parent, Vector3 localPos, Quaternion localRot)
            => m_View.transform.SetParent(parent, localPos, localRot);

        public void StartPrimaryFire() => m_PrimaryAction.StartAction();

        public void StopPrimaryFire() => m_PrimaryAction.StopAction();

        public void TryExecuteUltimate() => UltimateAction.TryExecute();

        private void UpdateState()
        {
            switch (m_Model.State)
            {
                case TankState.Idle:
                    if (Rigidbody.velocity.sqrMagnitude > 0.05f)
                    {
                        m_Model.State = TankState.Moving;
                        OnTankStateChangedToDriving();
                    }
                    break;
                case TankState.Moving:
                    if (Rigidbody.velocity.sqrMagnitude <= 0.05f)
                    {
                        m_Model.State = TankState.Idle;
                        OnTankStateChangedToIdle();
                    }
                    break;
            }
        }

        private void UpdateMoveSound()
        {
            if (m_Model.State != TankState.Moving)
                return;

            m_View.AudioView.UpdateEngineDrivingClipPitch(
                    Mathf.Lerp(0, 1, Mathf.InverseLerp(0, m_Model.TankData.MaxSpeed, m_Model.CurrentMoveSpeed)));
        }

        private void OnTankStateChangedToIdle() =>
            m_View.AudioView.PlayEngineIdleClip(m_Model.TankData.EngineIdleClip);

        private void OnTankStateChangedToDriving() =>
            m_View.AudioView.PlayEngineDrivingClip(m_Model.TankData.EngineDrivingClip);

        private void OnTankStateChangedToDead()
        {
            ToggleActorVisibility(false);

            m_Model.Reset();
            m_PrimaryAction.Disable();
            m_UltimateAction.Disable();

            m_View.AudioView.StopEngineAudio();

            OnEntityInitialized = null;

            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;

            SetParentOfView(m_Pool.TankContainer, Vector3.zero, Quaternion.identity);

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);

            m_Pool.ReturnTank(this);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Charge the ultimate action by the given amount using the inspector of a view.
        /// </summary>
        public void ChargeUltimate(float amount) => m_UltimateAction.Charge(amount);
#endif
    }
}

