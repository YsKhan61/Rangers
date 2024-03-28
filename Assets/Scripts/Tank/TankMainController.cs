using BTG.EventSystem;
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
    public class TankMainController
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
        private TankMovementController m_MovementController;
        private TankChargedFiringController m_Firing;
        private TankUltimateController m_UltimateController;
        private TankHealthController m_HealthController;

        public Transform Transform => m_View.transform;
        public Rigidbody Rigidbody => m_View.RigidBody;

        public Transform CameraTarget => m_View.CameraTarget;

        public Transform FirePoint => m_View.FirePoint;


        private TankPool m_Pool;


        public TankMainController(TankDataSO tankData, TankPool pool)
        {
            m_Pool = pool;

            m_Model = new TankModel(tankData, this);
            m_View = Object.Instantiate(tankData.TankViewPrefab, m_Pool.TankContainer);
            m_View.SetController(this);


            m_MovementController = new TankMovementController(this);
            m_Firing = new TankChargedFiringController(m_Model, m_View);
            m_UltimateController = new TankUltimateController(this, m_Model.TankData.UltimateActionFactory);
            m_HealthController = new TankHealthController(m_Model, this);

            m_Model.State = TankState.Idle;
            OnTankStateChangedToIdle();
        }

        public void Init()
        {
            m_View.gameObject.SetActive(true);
            m_Model.State = TankState.Idle;
        }


        public void FixedUpdate()
        {
            m_MovementController?.FixedUpdate();
        }

        public void Update()
        {
            UpdateState();

            m_MovementController?.Update();
            m_Firing?.Update();

            UpdateMoveSound();
        }

        public void OnDestroy()
        {
            m_Firing?.OnDestroy();
            m_UltimateController?.OnDestroy();
        }

        public void OnDead()
        {
            m_Firing?.OnDestroy();
            m_UltimateController?.OnDestroy();
            m_View.gameObject.SetActive(false);

            SetState(TankState.Dead);
            OnTankStateChangedToDead();

            m_Model.Reset();
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;

            m_Pool.ReturnTank(this);
        }

        public void SetMoveValue(float value)
        {
            m_Model.MoveInputValue = value;
        }

        public void SetRotateValue(float value)
        {
            m_Model.RotateInputValue = value;
        }

        public void StartTankFiring()
        {
            m_Firing.OnFireStarted();
        }

        public void EndTankFiring()
        {
            m_Firing.OnFireEnded();
        }

        public void TakeDamage(int damage)
        {
            m_HealthController.TakeDamage(damage);
        }

        public void SubscribeToOnTankShootEvent(Action<float> onTankShoot)
        {
            m_Firing.OnTankShoot += onTankShoot;
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

        public void SubscribeToFullyChargedEvent(Action onFullyCharged)
        {
            m_UltimateController.SubscribeToFullyChargedEvent(onFullyCharged);
        }

        public void ExecuteUltimateAction()
        {
            m_UltimateController.ExecuteUltimateAction();
        }

        public void ShowGraphics()
        {
            m_View.Show();
        }

        public void HideGraphics()
        {
            m_View.Hide();
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

