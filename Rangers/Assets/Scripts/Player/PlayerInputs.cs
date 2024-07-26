using BTG.Utilities;
using System;
using UnityEngine.InputSystem;


namespace BTG.Player
{
    public class PlayerInputs : IUpdatable, IDestroyable
    {
        public event Action<float> OnMoveInput;
        public event Action<float> OnRotateInput;
        public event Action OnPrimaryActionInputStarted;
        public event Action OnPrimaryActionInputCanceled;
        public event Action OnUltimateInputPerformed;


        private InputControls m_InputControls;
        private InputAction m_MoveInputAction;
        private InputAction m_RotateInputAction;

        public void Initialize()
        {
            m_InputControls = new InputControls();
            m_InputControls.Enable();
            m_InputControls.Player.Enable();
            m_InputControls.UI.Enable();

            m_MoveInputAction = m_InputControls.Player.MoveAction;
            m_RotateInputAction = m_InputControls.Player.RotateAction;

            m_InputControls.Player.Fire.started += OnStartedPrimaryActionInput;
            m_InputControls.Player.Fire.canceled += OnCanceledPrimaryActionInput;
            m_InputControls.Player.UltimateAction.performed += OnPerformedUltimateInput;

            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);
        }


        public void Update()
        {
            OnMoveInput?.Invoke(m_MoveInputAction.ReadValue<float>());
            OnRotateInput?.Invoke(m_RotateInputAction.ReadValue<float>());
        }

        public void Destroy()
        {
            m_InputControls.Player.Fire.started -= OnStartedPrimaryActionInput;
            m_InputControls.Player.Fire.canceled -= OnCanceledPrimaryActionInput;
            m_InputControls.Player.UltimateAction.performed -= OnPerformedUltimateInput;

            m_InputControls.Player.Disable();
            m_InputControls.UI.Disable();
            m_InputControls.Disable();

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }

        private void OnStartedPrimaryActionInput(InputAction.CallbackContext context)
        {
            OnPrimaryActionInputStarted?.Invoke();
        }

        private void OnCanceledPrimaryActionInput(InputAction.CallbackContext context)
        {
            OnPrimaryActionInputCanceled?.Invoke();
        }

        private void OnPerformedUltimateInput(InputAction.CallbackContext context)
        {
            OnUltimateInputPerformed?.Invoke();
        }
    }
}

