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
    public class TankController
    {
        public enum TankState
        {
            Idle,
            Driving
        }

        // dependencies
        private TankModel m_Model;
        public TankModel TankModel => m_Model;
        private TankView m_View;
        private TankMovementController m_MovementController;
        private TankChargedFiringController m_Firing;
        private TankUltimateController m_UltimateController;
        private TankHealthController m_HealthController;

        public Transform Transform => m_View.transform;
        public Rigidbody Rigidbody => m_View.RigidBody;

        public Transform CameraTarget => m_View.CameraTarget;

        public Transform FirePoint => m_View.FirePoint;

        public TankController(TankDataSO tankData)
        {
            m_Model = new TankModel(tankData, this);
            m_View = Object.Instantiate(tankData.TankViewPrefab);
            m_View.SetController(this);

            m_MovementController = new TankMovementController(this);
            m_Firing = new TankChargedFiringController(m_Model, m_View);
            m_UltimateController = new TankUltimateController(this, m_Model.TankData.UltimateActionFactory);
            m_HealthController = new TankHealthController(m_Model);

            m_Model.State = TankState.Idle;
            OnTankStateChangedToIdle();
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
                        m_Model.State = TankState.Driving;
                        OnTankStateChangedToDriving();
                    }
                    break;
                case TankState.Driving:
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
    }
}

