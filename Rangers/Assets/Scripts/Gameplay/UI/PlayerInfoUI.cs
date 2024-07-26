using BTG.Utilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BTG.Gameplay.UI
{
    public class PlayerInfoUI : MonoBehaviour
    {
        [SerializeField]
        StringDataSO m_PlayerName;

        [SerializeField]
        SpriteDataSO m_PlayerIcon;

        [SerializeField]
        IntIntEventChannelSO m_PlayerHealthEventChannel;

        [SerializeField, Tooltip("The UI panel that shows the info")]
        GameObject m_Panel;

        [SerializeField]
        private TextMeshProUGUI m_PlayerNameText;

        [SerializeField]
        private Image m_Image;

        [SerializeField]
        private Slider m_HealthBar;

        [SerializeField]
        private TextMeshProUGUI m_HealthText;

        [SerializeField]
        private Image m_HealthBarFill;

        [SerializeField, Tooltip("Color when health is zero")]
        private Color m_Color1;

        [SerializeField, Tooltip("Color when health is full")]
        private Color m_Color2;

        private void OnEnable()
        {
            m_PlayerName.OnValueChanged += OnPlayerNameDataChanged;
            m_PlayerIcon.OnValueChanged += OnPlayerIconDataChanged;
            m_PlayerHealthEventChannel.OnEventRaised += UpdateHealth;
        }

        private void Start()
        {
            Hide();
        }

        private void OnDisable()
        {
            m_PlayerName.OnValueChanged -= OnPlayerNameDataChanged;
            m_PlayerIcon.OnValueChanged -= OnPlayerIconDataChanged;
            m_PlayerHealthEventChannel.OnEventRaised -= UpdateHealth;
        }

        private void OnPlayerNameDataChanged() => m_PlayerNameText.text = m_PlayerName.Value;

        private void OnPlayerIconDataChanged()
        {
            SetTankIcon(m_PlayerIcon.Value);
            Show();
        }

        private void SetTankIcon(Sprite icon) => m_Image.sprite = icon;

        private void UpdateHealth(int currentHealth, int maxHealth)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            m_HealthBar.value = fillAmount;
            m_HealthBarFill.color = Color.Lerp(m_Color1, m_Color2, fillAmount);
            m_HealthText.text = $"{currentHealth}/{maxHealth}";
        }

        private void Show() => m_Panel.SetActive(true);

        private void Hide() => m_Panel.SetActive(false);
    }
}