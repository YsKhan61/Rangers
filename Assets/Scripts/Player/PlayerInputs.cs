
using BTG.Inputs;
using BTG.Tank;
using UnityEngine.InputSystem;


namespace BTG.Player
{
    public class PlayerInputs
    {
        private InputControls m_InputControls;

        private InputAction m_MoveInputAction;
        private InputAction m_RotateInputAction;

        private TankController m_TankController;

        public PlayerInputs(TankController tankController)
        {
            m_TankController = tankController;
        }

        public void Start()
        {
            m_InputControls = new InputControls();
            m_InputControls.Enable();
            m_InputControls.Player.Enable();

            m_MoveInputAction = m_InputControls.Player.MoveAction;
            m_RotateInputAction = m_InputControls.Player.RotateAction;

            m_InputControls.Player.Fire.started += OnFireInputStarted;
            m_InputControls.Player.Fire.canceled += OnFireInputActionCanceled;
            m_InputControls.Player.UltimateAction.performed += OnUltimateInputPerformed;
        }


        public void Update()
        {
            m_TankController.MoveInputValue = m_MoveInputAction.ReadValue<float>();
            m_TankController.RotateInputValue = m_RotateInputAction.ReadValue<float>();
        }

        public void OnDestroy()
        {
            m_InputControls.Player.Fire.started -= OnFireInputStarted;
            m_InputControls.Player.Fire.canceled -= OnFireInputActionCanceled;
            m_InputControls.Player.UltimateAction.performed -= OnUltimateInputPerformed;

            m_InputControls.Player.Disable();
            m_InputControls.Disable();
        }

        private void OnFireInputStarted(InputAction.CallbackContext context)
        {
            m_TankController.TankFiring.OnFireStarted();
        }

        private void OnFireInputActionCanceled(InputAction.CallbackContext context)
        {
            m_TankController.TankFiring.OnFireEnded();
        }

        private void OnUltimateInputPerformed(InputAction.CallbackContext context)
        {
            m_TankController.TankUltimateController.ExecuteUltimateAction();
        }
    }
}

