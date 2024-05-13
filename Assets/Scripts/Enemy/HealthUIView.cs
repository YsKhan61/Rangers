using UnityEngine;
using UnityEngine.UI;

namespace BTG.Enemy
{
    /// <summary>
    /// This class is responsible for updating the health UI of the enemy.
    /// </summary>
    public class HealthUIView : MonoBehaviour
    {
        [SerializeField] private Slider m_BarSlider;
        [SerializeField] private Image m_BackgroundImage;
        [SerializeField] private Image m_BarImage;

        /// <summary>
        /// Initializes the health UI with the given colors.
        /// </summary>
        /// <param name="backgroundCol">sets the background color of the slider</param>
        /// <param name="fillColor">sets the foreground color of the slider</param>
        public void Initialize(Color backgroundCol, Color fillColor)
        {
            m_BackgroundImage.color = backgroundCol;
            m_BarImage.color = fillColor;
        }

        /// <summary>
        /// Updates the health UI with the given value.
        /// </summary>
        /// <param name="value">the value between 0 and 1 that will be set to the slider</param>
        public void UpdateHealthUI(float value)
        {
            m_BarSlider.value = value;
        }

        /// <summary>
        /// Toggle the visibility of the health UI.
        /// </summary>
        /// <param name="isVisible"></param>
        public void ToggleVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}