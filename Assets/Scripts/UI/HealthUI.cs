using BTG.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace BTG.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField]
        SpriteDataSO m_PlayerIconData;

        [SerializeField]
        IntIntEventChannelSO m_PlayerHealthEventChannel;

        [SerializeField, Tooltip("The UI panel that shows the info")]
        GameObject m_Panel;

        [SerializeField]
        private Image m_TankIcon;

        [SerializeField]
        private Slider m_HealthBar;

        [SerializeField]
        private Image m_HealthBarFill;

        [SerializeField, Tooltip("Color when health is zero")]
        private Color m_Color1;

        [SerializeField, Tooltip("Color when health is full")]
        private Color m_Color2;

        private void OnEnable()
        {
            m_PlayerIconData.OnValueChanged += OnPlayerIconDataChanged;
            m_PlayerHealthEventChannel.OnPlayerHealthUpdated += UpdateHealth;
        }

        private void Start()
        {
            Hide();
        }

        private void OnDisable()
        {
            m_PlayerIconData.OnValueChanged -= OnPlayerIconDataChanged;
            m_PlayerHealthEventChannel.OnPlayerHealthUpdated -= UpdateHealth;
        }

        private void OnPlayerIconDataChanged(Sprite icon)
        {
            SetTankIcon(icon);
            Show();
        }

        private void SetTankIcon(Sprite icon)
        {
            m_TankIcon.sprite = icon;
        }

        private void UpdateHealth(int currentHealth, int maxHealth)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            Debug.Log("Fill Amount: " + fillAmount);
            m_HealthBar.value = fillAmount;
            m_HealthBarFill.color = Color.Lerp(m_Color1, m_Color2, fillAmount);
        }

        private void Show()
        {
            m_Panel.SetActive(true);
        }

        private void Hide()
        {
            m_Panel.SetActive(false);
        }
    }
}