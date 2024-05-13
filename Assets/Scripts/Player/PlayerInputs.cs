using BTG.Utilities;
using UnityEngine.InputSystem;


namespace BTG.Player
{
    public class PlayerInputs : IUpdatable, IDestroyable
    {
        private InputControls m_InputControls;

        private InputAction m_MoveInputAction;
        private InputAction m_RotateInputAction;

        private PlayerTankController m_Controller;

        public PlayerInputs(PlayerTankController controller)
        {
            m_Controller = controller;
        }

        public void Initialize()
        {
            m_InputControls = new InputControls();
            m_InputControls.Enable();
            m_InputControls.Player.Enable();
            m_InputControls.UI.Enable();

            m_MoveInputAction = m_InputControls.Player.MoveAction;
            m_RotateInputAction = m_InputControls.Player.RotateAction;

            m_InputControls.Player.Fire.started += OnFireInputStarted;
            m_InputControls.Player.Fire.canceled += OnFireInputActionCanceled;
            m_InputControls.Player.UltimateAction.performed += OnUltimateInputPerformed;

            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);
        }


        public void Update()
        {
            m_Controller.SetMoveValue(m_MoveInputAction.ReadValue<float>());
            m_Controller.SetRotateValue(m_RotateInputAction.ReadValue<float>());
        }

        public void Destroy()
        {
            m_InputControls.Player.Fire.started -= OnFireInputStarted;
            m_InputControls.Player.Fire.canceled -= OnFireInputActionCanceled;
            m_InputControls.Player.UltimateAction.performed -= OnUltimateInputPerformed;

            m_InputControls.Player.Disable();
            m_InputControls.UI.Disable();
            m_InputControls.Disable();

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }

        private void OnFireInputStarted(InputAction.CallbackContext context)
        {
            m_Controller.StartFire();
        }

        private void OnFireInputActionCanceled(InputAction.CallbackContext context)
        {
            m_Controller.StopFire();
        }

        private void OnUltimateInputPerformed(InputAction.CallbackContext context)
        {
            m_Controller.TryExecuteUltimate();
        }
    }
}

