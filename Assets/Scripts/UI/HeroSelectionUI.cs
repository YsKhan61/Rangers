using BTG.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BTG.UI
{ 
    public class HeroSelectionUI : MonoBehaviour
    {
        [SerializeField, Tooltip("The UI panel of tank selection")]
        private GameObject m_Panel;

        [SerializeField]
        private IntDataSO m_TankIDSelectedData;

        [SerializeField]
        private IntDataSO m_PlayerDeathData;

        [SerializeField]
        private InputActionReference m_InputActionReference;

        private void OnEnable()
        {
            m_PlayerDeathData.OnValueChanged += OnPlayerDeath;
            m_InputActionReference.action.performed += OnToggleVisibilityInputPerformed;
            m_InputActionReference.action.Enable();
        }

        private void Start() => ShowPanel();

        private void OnDisable()
        {
            m_PlayerDeathData.OnValueChanged -= OnPlayerDeath;
            m_InputActionReference.action.performed -= OnToggleVisibilityInputPerformed;
            m_InputActionReference.action.Disable();
        }

        public void TankIDSelect(int id)
        {
            m_TankIDSelectedData.Value = id;
            HidePanel();
        }

        private void ShowPanel() => m_Panel.SetActive(true);

        private void HidePanel() => m_Panel.SetActive(false);

        private void OnPlayerDeath(int _) => ShowPanel();

        private void OnToggleVisibilityInputPerformed(InputAction.CallbackContext context)
        {
            if (m_Panel.activeSelf)
                HidePanel();
            else
                ShowPanel();
        }
    }
}
