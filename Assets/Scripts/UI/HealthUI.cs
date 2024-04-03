using UnityEngine;
using UnityEngine.UI;

namespace BTG.UI
{
    public class HealthUI : MonoBehaviour
    {
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

        public void SetTankIcon(Sprite icon)
        {
            m_TankIcon.sprite = icon;
        }

        public void UpdateHealth(int currentHealth, int maxHealth)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            Debug.Log("Fill Amount: " + fillAmount);
            m_HealthBar.value = fillAmount;
            m_HealthBarFill.color = Color.Lerp(m_Color1, m_Color2, fillAmount);
        }
    }
}