using BTG.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


namespace BTG.Gameplay.UI
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField, Tooltip("The UI panel that shows the player stats")]
        GameObject m_PlayerStatsPanel;

        [SerializeField]
        PlayerStatsSO m_PlayerStatsData;

        [SerializeField]
        TextMeshProUGUI m_PlayerNameText;

        [SerializeField]
        TextMeshProUGUI m_DeathCountText;

        [SerializeField]
        TextMeshProUGUI m_EliminatedEnemiesCountText;

        [SerializeField]
        InputActionReference m_InputActionReference;

        void OnEnable()
        {
            m_PlayerStatsData.PlayerName.OnValueChanged += OnPlayerNameChanged;
            m_PlayerStatsData.DeathCount.OnValueChanged += OnDeathCountChanged;
            m_PlayerStatsData.EliminatedEnemiesCount.OnValueChanged += OnEliminatedEnemiesCountChanged;

            m_InputActionReference.action.Enable();
            m_InputActionReference.action.performed += OnInputActionPerformed;
        }

        void OnDisable()
        {
            m_PlayerStatsData.PlayerName.OnValueChanged -= OnPlayerNameChanged;
            m_InputActionReference.action.performed -= OnInputActionPerformed;
            m_InputActionReference.action.Disable();

            m_PlayerStatsData.DeathCount.OnValueChanged -= OnDeathCountChanged;
            m_PlayerStatsData.EliminatedEnemiesCount.OnValueChanged -= OnEliminatedEnemiesCountChanged;
        }

        void Start()
        {
            HidePanel();
        }

        void ShowPanel() => m_PlayerStatsPanel.SetActive(true);

        void HidePanel() => m_PlayerStatsPanel.SetActive(false);

        void OnPlayerNameChanged() => m_PlayerNameText.text = m_PlayerStatsData.PlayerName.Value;

        void OnDeathCountChanged() => m_DeathCountText.text = m_PlayerStatsData.DeathCount.Value.ToString();

        void OnEliminatedEnemiesCountChanged() 
            => m_EliminatedEnemiesCountText.text = m_PlayerStatsData.EliminatedEnemiesCount.Value.ToString();
    
        void OnInputActionPerformed(InputAction.CallbackContext context)
        {
            if (m_PlayerStatsPanel.activeSelf)
                HidePanel();
            else
                ShowPanel();
        }
    }
}