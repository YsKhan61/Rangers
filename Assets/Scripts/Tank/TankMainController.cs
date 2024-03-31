using BTG.EventSystem;
using BTG.Tank.UltimateAction;
using BTG.Utilities;
using System;
using UnityEngine;
using Object = UnityEngine.Object;


namespace BTG.Tank
{
    /// <summary>
    /// The main controller for the tank. It handles the communications between Model, View and other controllers such as 
    /// TankMovementController, TankFiringController and TankUltimateController.
    /// It is like a Facade for the tank.
    /// </summary>
    public class TankMainController : IUpdatable, IDestroyable
    {
        public enum TankState
        {
            Idle,
            Driving,
            Dead
        }

        // dependencies
        private TankModel m_Model;
        public TankModel Model => m_Model;
        private TankView m_View;

        private TankChargedFiringController m_FiringController;
        public TankChargedFiringController FiringController => m_FiringController;

        private TankUltimateController m_UltimateController;
        public TankUltimateController UltimateController => m_UltimateController;

        private TankHealthController m_HealthController;
        public TankHealthController HealthController => m_HealthController;

        public Transform Transform => m_View.transform;
        public Rigidbody Rigidbody => m_View.RigidBody;

        public Transform CameraTarget => m_View.CameraTarget;

        public Transform FirePoint => m_View.FirePoint;

        public IDamageable Damageable => m_View;
        public LayerMask OppositionLayerMask => m_Model.OppositionLayer;

        private TankPool m_Pool;

        /// <summary>
        /// This constructor creates all the respective properties of the tank that are mandatory
        /// for the tank to function properly.
        /// TankView, TankModel, TankMovementController, TankFiringController, TankUltimateController, TankHealthController
        /// </summary>
        /// <param name="tankData"></param>
        /// <param name="pool"></param>
        public TankMainController(TankDataSO tankData, TankPool pool)
        {
            m_Pool = pool;

            m_Model = new TankModel(tankData, this);
            m_View = Object.Instantiate(tankData.TankViewPrefab, m_Pool.TankContainer);
            m_View.SetController(this);

            m_FiringController = new TankChargedFiringController(m_Model, m_View);
            m_UltimateController = new TankUltimateController(this, m_Model.TankData.UltimateActionFactory);
            m_HealthController = new TankHealthController(m_Model, this);
        }

        public void Init()
        {
            m_Model.State = TankState.Idle;
            OnTankStateChangedToIdle();

            ToggleTankVisibility(true);
            Rigidbody.WakeUp();

            m_UltimateController.EnableUltimate();
            

            UnityCallbacks.Instance.Register(this as IUpdatable);
            UnityCallbacks.Instance.Register(this as IDestroyable);
        }

        public void SetLayers(int selfLayer, int oppositionLayer)
        {
            m_View.gameObject.layer = selfLayer;
            m_Model.OppositionLayer = 1 << oppositionLayer;
        }


        public void Update()
        {
            UpdateState();

            m_FiringController?.Update();

            UpdateMoveSound();
        }

        public void OnDestroy()
        {
            m_FiringController?.OnDestroy();
            m_UltimateController?.OnDestroy();
        }

        public void OnDead()
        {
            ToggleTankVisibility(false);

            m_Model.Reset();
            m_UltimateController.DisableUltimate();

            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.Sleep();

            SetState(TankState.Dead);
            OnTankStateChangedToDead();

            UnityCallbacks.Instance.Unregister(this as IUpdatable);
            UnityCallbacks.Instance.Unregister(this as IDestroyable);

            m_Pool.ReturnTank(this);
        }

        public void SubscribeToOnTankShootEvent(Action<float> onTankShoot)
        {
            m_FiringController.OnTankShoot += onTankShoot;
        }

        public void SubscribeToUltimateActionAssignedEvent(Action<string> onUltimateActionAssigned)
        {
            m_UltimateController.SubscribeToUltimateActionAssignedEvent(onUltimateActionAssigned);
        }

        public void SubscribeToUltimateExecutedEvent(Action onUltimateExecuted)
        {
            m_UltimateController.SubscribeToUltimateExecutedEvent(onUltimateExecuted);
        }

        public void SubscribeToCameraShakeEvent(Action<float> onCameraShake)
        {
            m_UltimateController.SubscribeToCameraShakeEvent(onCameraShake);
        }

        public void SubscribeToChargeUpdatedEvent(Action<int> onChargeUpdated)
        {
            m_UltimateController.SubscribeToChargeUpdatedEvent(onChargeUpdated);
        }

        public void SubscribeToFullyChargedEvent(Action<IUltimateAction> onFullyCharged)
        {
            m_UltimateController.SubscribeToFullyChargedEvent(onFullyCharged);
        }

        /// <summary>
        /// True - Make tank visible, False - Make tank invisible
        /// </summary>
        /// <param name="value"></param>
        public void ToggleTankVisibility(bool value)
        {
            m_View.ToggleVisible(value);
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

        private void SetState(TankState state)
        {
            m_Model.State = state;
        }

        private void UpdateMoveSound()
        {
            if (m_Model.State == TankState.Driving)
            {
                m_View.TankAudio.UpdateEngineDrivingClipPitch(
                    Mathf.Lerp(0, 1, Mathf.InverseLerp(0, m_Model.TankData.MaxSpeed, m_Model.CurrentMoveSpeed)));
            }
        }

        private void OnTankStateChangedToIdle()
        {
            m_View.TankAudio.PlayEngineIdleClip(m_Model.TankData.EngineIdleClip);
        }

        private void OnTankStateChangedToDriving()
        {
            m_View.TankAudio.PlayEngineDrivingClip(m_Model.TankData.EngineDrivingClip);
        }

        private void OnTankStateChangedToDead()
        {
            m_View.TankAudio.StopEngineAudio();
            EventService.Instance.OnTankDead?.InvokeEvent(m_Model.IsPlayer);
        }
    }
}

