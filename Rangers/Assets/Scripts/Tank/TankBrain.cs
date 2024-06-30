using BTG.Actions.PrimaryAction;
using BTG.Actions.UltimateAction;
using BTG.Entity;
using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using System;
using UnityEngine;
using VContainer;


namespace BTG.Tank
{
    /// <summary>
    /// The TankBrain for the tank. It handles the communications between Model, View and other controllers such as 
    /// PrimaryAction, UltimateAction.
    /// </summary>
    public class TankBrain : IEntityBrain
    {
        public event Action<Sprite> OnEntityInitialized;
        public event Action<bool> OnEntityVisibilityToggled;
        public event Action<CameraShakeEventData> OnPlayerCameraShake;

        public enum TankState
        {
            Idle,
            Moving,
            Deactive
        }

        public TagSO Tag => m_Model.TankData.Tag;

        private TankModel m_Model;
        public IEntityModel Model => m_Model;
        public IEntityController Controller { get; private set; }
        private TankView m_View;
        private IPrimaryAction m_PrimaryAction;
        public IPrimaryAction PrimaryAction => m_PrimaryAction;
        private IUltimateAction m_UltimateAction;
        public IUltimateAction UltimateAction => m_UltimateAction;
        public Transform Transform => m_View.transform;
        public Transform CameraTarget => m_View.CameraTarget;
        public Transform FirePoint => m_View.FirePoint;
        public LayerMask OppositionLayerMask => m_Model.OppositionLayer;
        public Collider DamageCollider => m_View.DamageCollider;
        public IDamageableView Damageable { get; private set; }

        public bool IsPlayer => m_Model.IsPlayer;
        public bool IsNetworkPlayer => m_Model.IsNetworkPlayer;
        public ulong NetworkObjectId => m_Model.NetworkObjectId;
        public float CurrentMoveSpeed => Controller.Velocity.magnitude;

        [Inject]
        private PrimaryActionFactoryContainerSO m_PrimaryActionFactoryContainer;

        [Inject]
        private UltimateActionFactoryContainerSO m_UltimateActionFactoryContainer;

        /*[Inject]
        private AudioPool m_AudioPool;*/

        private TankPool m_Pool;

        private TankBrain() { }

        public class Builder
        {
            private TankModel tankModel;
            private TankPool tankPool;
            private TankView tankView;

            public Builder WithTankModel(TankModel model)
            {
                tankModel = model;
                return this;
            }

            public Builder WithTankPool(TankPool pool)
            {
                tankPool = pool;
                return this;
            }

            public Builder WithTankView(TankView view)
            {
                tankView = view;
                return this;
            }

            public TankBrain Build()
            {
                return new TankBrain
                {
                    m_Model = tankModel,
                    m_Pool = tankPool,
                    m_View = tankView,
                };
            }
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

            m_PrimaryAction.Enable();
            m_UltimateAction.Enable();

            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);

            _ = HelperMethods.InvokeInNextFrame(() => OnEntityInitialized?.Invoke(m_Model.Icon));
        }

        public void InitNonServer()
        {
            m_Model.State = TankState.Idle;
            OnTankStateChangedToIdle();

            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);
        }

        public void DeInit()
        {
            m_Model.Reset();

            m_PrimaryAction.Disable();
            m_UltimateAction.Disable();

            m_View.AudioView.StopEngineAudio();

            OnEntityInitialized = null;
            OnEntityVisibilityToggled = null;

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);

            m_Pool.ReturnTank(m_View);
        }

        public void DeInitNonServer()
        {
            m_Model.Reset();

            m_View.AudioView.StopEngineAudio();

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);

            m_Pool.ReturnTank(m_View);
        }

        public void SetController(IEntityController controller) => Controller = controller;

        public void CreatePrimaryAction(TagSO primaryTag = null)
        {
            if (primaryTag == null)
                m_PrimaryAction = GetPrimaryActionFactory(m_Model.TankData.PrimaryTag).GetItem();
            else
                m_PrimaryAction = GetPrimaryActionFactory(primaryTag).GetItem();

            if (m_PrimaryAction == null)
                Debug.LogError("Primary action is null");

            m_PrimaryAction.SetActor(this);
        }

        public void CreateNetworkPrimaryAction(TagSO primaryTag = null)
        {
            if (primaryTag == null)
                m_PrimaryAction = GetPrimaryActionFactory(m_Model.TankData.PrimaryTag).GetNetworkItem();
            else
                m_PrimaryAction = GetPrimaryActionFactory(primaryTag).GetNetworkItem();

            if (m_PrimaryAction == null)
                Debug.LogError("Primary action is null");

            m_PrimaryAction.SetActor(this);
        }

        /// <summary>
        /// Create the ultimate action for the tank.
        /// If Tag is not provided, it will use the default tag from the tank data.
        /// This method is called when the tank needs to create new ultimate action.
        /// </summary>
        public void CreateUltimateAction(TagSO ultimateTag = null)
        {
            if (ultimateTag == null)
                m_UltimateAction = GetUltimateActionFactory(m_Model.TankData.UltimateTag).GetItem();
            else
                m_UltimateAction = GetUltimateActionFactory(ultimateTag).GetItem();

            if (m_UltimateAction == null)
                Debug.LogError("Ultimate action is null");

            m_UltimateAction.SetActor(this);
        }

        public void CreateNetworkUltimateAction(TagSO ultimateTag = null)
        { 
            if (ultimateTag == null)
                m_UltimateAction = GetUltimateActionFactory(m_Model.TankData.UltimateTag).GetNetworkItem();
            else
                m_UltimateAction = GetUltimateActionFactory(ultimateTag).GetNetworkItem();
        
            if (m_UltimateAction == null)
                Debug.LogError("Ultimate action is null");

            m_UltimateAction.SetActor(this);
        }

        public void SetDamageable(IDamageableView damageable) => Damageable = damageable;

        public void SetOppositionLayerMask(LayerMask layer) => m_Model.OppositionLayer = layer;

        public void Update()
        {
            UpdateState();
            UpdateMoveSound();
        }

        public void Destroy()
        {
            OnEntityInitialized = null;
        }

        public void ToggleActorVisibility(bool value)
        {
            OnEntityVisibilityToggled?.Invoke(value);
            m_View.ToggleVisible(value);
            m_View.ToggleMuteAudio(!value);

            DamageCollider.enabled = value;
        }
        public void SetParentOfView(Transform parent, Vector3 localPos, Quaternion localRot)
            => m_View.transform.SetParent(parent, localPos, localRot);

        public void StartPrimaryAction() => m_PrimaryAction.StartAction();

        public void StopPrimaryAction() => m_PrimaryAction.StopAction();

        public void AutoStartStopPrimaryAction(int stopTime) => m_PrimaryAction.AutoStartStopAction(stopTime);

        public bool TryExecuteUltimate() => UltimateAction.TryExecute();

        public void ExecuteRagdollEffectEvent()
        {
            if (m_Model.IsNetworkPlayer)
            {
                EventBus<NetworkEffectEventData>.Invoke(new NetworkEffectEventData
                {
                    OwnerClientOnly = false,
                    FollowNetworkObject = false,
                    EffectTagNetworkGuid = m_Model.TankData.Tag.Guid.ToNetworkGuid(),
                    EffectPosition = Transform.position,
                });
            }
            else
            {
                EventBus<EffectEventData>.Invoke(new EffectEventData
                {
                    EffectTag = m_Model.TankData.Tag,
                    EffectPosition = Transform.position,
                });
            }
        }

        public void RaisePlayerCamShakeEvent(CameraShakeEventData camShakeData) => OnPlayerCameraShake?.Invoke(camShakeData);

        public void InitializeChargingAndShootingClips(AudioClip chargingClip, AudioClip shotFiredClip)
            => m_View.AudioView.InitializeChargingAndShootingClips(chargingClip, shotFiredClip);

        public void PlayChargingClip() => m_View.AudioView.PlayChargingClip();
        
        public void UpdateChargingClipPitch(float amount) => m_View.AudioView.UpdateChargingClipPitch(amount);
        
        public void PlayShotFiredClip() => m_View.AudioView.PlayShotFiredClip();
        // private void ExecuteDeadAudio() => m_AudioPool.GetAudioView().PlayOneShot(m_Model.TankData.DeathSoundClip, Transform.position);
        
        private void UpdateState()
        {
            switch (m_Model.State)
            {
                case TankState.Idle:
                    if (CurrentMoveSpeed > 0.05f)
                    {
                        m_Model.State = TankState.Moving;
                        OnTankStateChangedToDriving();
                    }
                    break;
                case TankState.Moving:
                    if (CurrentMoveSpeed <= 0.05f)
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
                    Mathf.Lerp(0, 1, Mathf.InverseLerp(0, m_Model.TankData.MaxSpeed, CurrentMoveSpeed)));
        }

        private void OnTankStateChangedToIdle() =>
            m_View.AudioView.PlayEngineIdleClip(m_Model.TankData.EngineIdleClip);

        private void OnTankStateChangedToDriving() =>
            m_View.AudioView.PlayEngineDrivingClip(m_Model.TankData.EngineDrivingClip);

        private PrimaryActionFactorySO GetPrimaryActionFactory(TagSO tag) =>
            m_PrimaryActionFactoryContainer.GetFactory(tag) as PrimaryActionFactorySO;

        private UltimateActionFactorySO GetUltimateActionFactory(TagSO tag) =>
            m_UltimateActionFactoryContainer.GetFactory(tag) as UltimateActionFactorySO;


#if UNITY_EDITOR
        /// <summary>
        /// Charge the ultimate action by the given amount using the inspector of a view.
        /// </summary>
        public void ChargeUltimate(float amount) => m_UltimateAction.Charge(amount);
#endif
    }
}

