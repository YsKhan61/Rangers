using BTG.EventSystem;
using BTG.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BTG.Gameplay.UI
{
    /// <summary>
    /// Script that manages the entity selection UI.
    /// Don't add the script to the entity selection UI panel.
    /// As the script is attached to the panel, it will be enabled and disabled when the panel is enabled and disabled.
    /// </summary>
    public class EntitySelectionUI : MonoBehaviour
    {
        [SerializeField, Tooltip("The UI panel of entity selection")]
        private GameObject m_Panel;

        [SerializeField]
        private TagDataSO m_EntityTagSelected;

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
        /// This method is called when a entity is selected by player
        /// Then the panel is hidden.
        /// </summary>
        /// <param name="tag">tag of the entity selected</param>
        public void EntityTagSelectAndHide(TagSO tag)
        {
            m_EntityTagSelected.Value = tag;
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
