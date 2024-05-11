using BTG.Actions.PrimaryAction;
using BTG.Actions.UltimateAction;
using BTG.Entity;
using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;


namespace BTG.Tank
{
    /// <summary>
    /// The TankBrain for the tank. It handles the communications between Model, View and other controllers such as 
    /// TankMovementController, TankFiringController and TankUltimateController.
    /// It is like a Facade/Mediator for the tank.
    /// </summary>
    public class TankBrain : IEntityTankBrain, IUpdatable, IDestroyable
    {
        public event Action<Sprite> OnEntityInitialized;
        public event Action OnAfterDeath;
        public event Action<bool> OnEntityVisibilityToggled;

        public enum TankState
        {
            Idle,
            Driving,
            Dead
        }


        private TankModel m_Model;
        IEntityTankModel IEntityTankBrain.Model => m_Model;

        private TankView m_View;
        private IPrimaryAction m_PrimaryAction;
        public IPrimaryAction PrimaryAction => m_PrimaryAction;
        
        private IUltimateAction m_UltimateAction;
        public IUltimateAction UltimateAction => m_UltimateAction;
        
        private TankHealthController m_HealthController;
        public IEntityHealthController HealthController => m_HealthController;
        public Transform Transform => m_View.transform;
        public Transform CameraTarget => m_View.CameraTarget;
        public Rigidbody Rigidbody { get; private set; }
        public Transform FirePoint => m_View.FirePoint;
        public IDamageable Damageable => m_View.Damageable;
        public LayerMask OppositionLayerMask => m_Model.OppositionLayer;

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
            m_HealthController = new TankHealthController(m_Model, this);
        }


        public void Init()
        {
            m_Model.State = TankState.Idle;

            OnTankStateChangedToIdle();
            ToggleActorVisibility(true);

            m_PrimaryAction.Enable();
            m_UltimateAction.Enable();
            m_HealthController.Reset();

            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);

            _ = RaiseInitializedEventAsync();
        }

        public void SetRigidbody(Rigidbody rb) => Rigidbody = rb;

        public void SetLayers(int selfLayer, int oppositionLayer)
        {
            m_View.SetDamageableLayer(selfLayer);
            m_Model.OppositionLayer = 1 << oppositionLayer;
        }

        public void Update()
        {
            UpdateState();
            UpdateMoveSound();
        }

        public void OnDestroy()
        {
            OnEntityInitialized = null;
        }

        public void Die()
        {
            SetState(TankState.Dead);
            OnTankStateChangedToDead();
        }


        /// <summary>
        /// True - Make tank visible, False - Make tank invisible
        /// </summary>
        /// <param name="value"></param>
        public void ToggleActorVisibility(bool value)
        {
            OnEntityVisibilityToggled?.Invoke(value);
            m_View.ToggleVisible(value);
        }
        public void SetParentOfView(Transform parent, Vector3 localPos, Quaternion localRot)
            => m_View.transform.SetParent(parent, localPos, localRot);

        public void StartPrimaryFire() => m_PrimaryAction.StartAction();

        public void StopPrimaryFire() => m_PrimaryAction.StopAction();

        public void ChargeUltimate(float amount) => m_UltimateAction.Charge(amount);

        public void TryExecuteUltimate() => UltimateAction.TryExecute();

        public void TakeDamage(int damage)
        {
            m_HealthController.TakeDamage(damage);
            if (m_Model.IsPlayer)
                EventBus<CameraShakeEvent>.Invoke(new CameraShakeEvent { ShakeAmount = 0.5f, ShakeDuration = 0.2f }); // OnPlayerCamShake?.Invoke(0.5f, 0.2f);           // shake values are hardcoded for now
        }

        private void UpdateState()
        {
            switch (m_Model.State)
            {
                case TankState.Idle:
                    if (Rigidbody.velocity.sqrMagnitude > 0.05f)
                    {
                        SetState(TankState.Driving);
                        OnTankStateChangedToDriving();
                    }
                    break;
                case TankState.Driving:
                    if (Rigidbody.velocity.sqrMagnitude <= 0.05f)
                    {
                        SetState(TankState.Idle);
                        OnTankStateChangedToIdle();
                    }
                    break;
            }
        }

        private void SetState(TankState state) => m_Model.State = state;

        private void UpdateMoveSound()
        {
            if (m_Model.State != TankState.Driving)
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

            OnAfterDeath?.Invoke();
            OnAfterDeath = null;

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this as IUpdatable);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this as IDestroyable);

            m_Pool.ReturnTank(this);
        }

        private async Task RaiseInitializedEventAsync()
        {
            await Task.Yield();
            OnEntityInitialized?.Invoke(m_Model.Icon);
        }
    }
}

