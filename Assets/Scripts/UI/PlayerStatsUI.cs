using BTG.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


namespace BTG.UI
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField, Tooltip("The UI panel that shows the player stats")]
        private GameObject m_PlayerStatsPanel;

        [SerializeField]
        private PlayerStatsSO m_PlayerStatsData;

        [SerializeField]
        private TextMeshProUGUI m_DeathCountText;

        [SerializeField]
        private TextMeshProUGUI m_EliminatedEnemiesCountText;

        [SerializeField]
        InputActionReference m_InputActionReference;

        private void OnEnable()
        {
            m_PlayerStatsData.DeathCount.OnValueChanged += OnDeathCountChanged;
            m_PlayerStatsData.EliminatedEnemiesCount.OnValueChanged += OnEliminatedEnemiesCountChanged;

            m_InputActionReference.action.Enable();
            m_InputActionReference.action.performed += OnInputActionPerformed;
        }

        private void OnDisable()
        {
            m_InputActionReference.action.performed -= OnInputActionPerformed;
            m_InputActionReference.action.Disable();

            m_PlayerStatsData.DeathCount.OnValueChanged -= OnDeathCountChanged;
            m_PlayerStatsData.EliminatedEnemiesCount.OnValueChanged -= OnEliminatedEnemiesCountChanged;
        }

        private void Start()
        {
            HidePanel();
        }

        private void ShowPanel() => m_PlayerStatsPanel.SetActive(true);

        private void HidePanel() => m_PlayerStatsPanel.SetActive(false);

        private void OnDeathCountChanged(int deathCount) => m_DeathCountText.text = deathCount.ToString();

        private void OnEliminatedEnemiesCountChanged(int eliminatedEnemiesCount) => m_EliminatedEnemiesCountText.text = eliminatedEnemiesCount.ToString();
    
        private void OnInputActionPerformed(InputAction.CallbackContext context)
        {
            if (m_PlayerStatsPanel.activeSelf)
                HidePanel();
            else
                ShowPanel();
        }
    }
}