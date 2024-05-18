using BTG.Actions.PrimaryAction;
using BTG.Actions.UltimateAction;
using BTG.AudioSystem;
using BTG.Effects;
using BTG.Entity;
using BTG.Utilities;
using BTG.Utilities.DI;
using System;
using UnityEngine;
using Object = UnityEngine.Object;


namespace BTG.Tank
{
    /// <summary>
    /// The TankBrain for the tank. It handles the communications between Model, View and other controllers such as 
    /// PrimaryAction, UltimateAction.
    /// </summary>
    public class TankBrain : IEntityTankBrain
    {
        public event Action<Sprite> OnEntityInitialized;
        public event Action<bool> OnEntityVisibilityToggled;

        public enum TankState
        {
            Idle,
            Moving,
            Deactive
        }

        public TagSO Tag => m_Model.TankData.Tag;

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
        public IDamageableView Damageable { get; private set; }

        public bool IsPlayer { get => m_Model.IsPlayer; set => m_Model.IsPlayer = value; }
        public float CurrentMoveSpeed => m_Model.CurrentMoveSpeed;

        [Inject]
        private PrimaryActionFactoryContainerSO m_PrimaryActionFactoryContainer;

        [Inject]
        private UltimateActionFactoryContainerSO m_UltimateActionFactoryContainer;

        [Inject]
        private RagdollFactoryContainerSO m_RagdollFactoryContainer;

        [Inject]
        private AudioPool m_AudioPool;

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
            DIManager.Instance.Inject(this);

            m_Pool = pool;

            m_Model = new TankModel(tankData, this);
            m_View = Object.Instantiate(tankData.TankViewPrefab, m_Pool.TankContainer);
            m_View.SetBrain(this);

            CreatePrimaryAction();
            CreateUltimateAction();
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

            Rigidbody.WakeUp();
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;

            m_PrimaryAction.Enable();
            m_UltimateAction.Enable();

            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);

            _ = HelperMethods.InvokeInNextFrame(() => OnEntityInitialized?.Invoke(m_Model.Icon));
        }

        public void CreatePrimaryAction(TagSO primaryTag = null)
        {
            if (primaryTag == null)
                m_PrimaryAction = m_PrimaryActionFactoryContainer.GetPrimaryAction(m_Model.TankData.PrimaryTag);
            else
                m_PrimaryAction = m_PrimaryActionFactoryContainer.GetPrimaryAction(primaryTag);

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
                m_UltimateAction = m_UltimateActionFactoryContainer.GetUltimateAction(m_Model.TankData.UltimateTag);
            else
                m_UltimateAction = m_UltimateActionFactoryContainer.GetUltimateAction(ultimateTag);

            m_UltimateAction.SetActor(this);
        }

        public void SetRigidbody(Rigidbody rb) => Rigidbody = rb;

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

        public void DeInit()
        {
            m_Model.State = TankState.Deactive;
            ToggleActorVisibility(false);

            Rigidbody.Sleep();

            m_Model.Reset();
            m_PrimaryAction.Disable();
            m_UltimateAction.Disable();

            m_View.AudioView.StopEngineAudio();

            OnEntityInitialized = null;
            OnEntityVisibilityToggled = null;

            SetParentOfView(m_Pool.TankContainer, Vector3.zero, Quaternion.identity);

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);

            m_Pool.ReturnTank(this);
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

        public void TryExecuteUltimate() => UltimateAction.TryExecute();

        public void ExecuteRagdollEffect() => m_RagdollFactoryContainer.GetAndExecuteRagdollEffect(this);

        public void OnDead()
        {
            ExecuteRagdollEffect();
            ExecuteDeadAudio();
        }

        private void ExecuteDeadAudio() => m_AudioPool.GetAudioView().PlayOneShot(m_Model.TankData.DeathSoundClip, Transform.position);

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


#if UNITY_EDITOR
        /// <summary>
        /// Charge the ultimate action by the given amount using the inspector of a view.
        /// </summary>
        public void ChargeUltimate(float amount) => m_UltimateAction.Charge(amount);
#endif
    }
}

