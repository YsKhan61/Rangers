using BTG.EventSystem;
using BTG.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BTG.UI
{
    /// <summary>
    /// Script that manages the tank selection UI.
    /// Don't add the script to the tank selection UI panel.
    /// As the script is attached to the panel, it will be enabled and disabled when the panel is enabled and disabled.
    /// </summary>
    public class HeroSelectionUI : MonoBehaviour
    {
        [SerializeField, Tooltip("The UI panel of tank selection")]
        private GameObject m_Panel;

        [SerializeField]
        private IntDataSO m_TankIDSelectedData;

        [SerializeField]
        private InputActionReference m_InputActionReference;

        private void OnEnable()
        {
            EventService.Instance.OnShowHeroSelectionUI.AddListener(ShowPanel);
            m_InputActionReference.action.performed += OnToggleVisibilityInputPerformed;
            m_InputActionReference.action.Enable();
        }

        private void Start() => ShowPanel();

        private void OnDisable()
        {
            EventService.Instance.OnShowHeroSelectionUI.RemoveListener(ShowPanel);
            m_InputActionReference.action.performed -= OnToggleVisibilityInputPerformed;
            m_InputActionReference.action.Disable();
        }

        /// <summary>
        /// This method is called when a tank is selected.
        /// </summary>
        /// <param name="id">id of the selected tank</param>
        public void TankIDSelect(int id)
        {
            m_TankIDSelectedData.Value = id;
            HidePanel();
        }

        private void ShowPanel() => m_Panel.SetActive(true);

        private void HidePanel() => m_Panel.SetActive(false);

        private void OnToggleVisibilityInputPerformed(InputAction.CallbackContext context)
        {
            if (m_Panel.activeSelf)
                HidePanel();
            else
                ShowPanel();
        }
    }
}
