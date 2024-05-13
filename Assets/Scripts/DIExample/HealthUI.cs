using BTG.Utilities;
using BTG.Utilities.DI;
using TMPro;
using UnityEngine;

namespace BTG.DIExample
{
    public class HealthUI : MonoBehaviour, IDependencyInjector
    {
        [Inject]
        IntDataSO healthData;

        [SerializeField]
        TextMeshProUGUI healthText;

        private void OnEnable()
        {
            healthData.OnValueChanged += UpdateHealthUI;
        }

        private void OnDisable()
        {
            healthData.OnValueChanged -= UpdateHealthUI;
        }

        private void UpdateHealthUI()
        {
            healthText.text = $"Health: {healthData.Value}";
        }
    }
}