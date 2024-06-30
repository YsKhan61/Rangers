using BTG.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BTG.Gameplay.UI
{
    public class PrimaryActionUI : MonoBehaviour
    {
        [SerializeField, Tooltip("EventChannel raised when the player's primary action is assigned")]
        TagEventChannelSO m_PrimaryActionAssignedEventChannel;

        [SerializeField, Tooltip("Event channel which will be raised when Entity's Primary Action starts")]
        private VoidEventChannelSO m_PrimaryActionStartedEventChannel;

        [SerializeField,Tooltip("EventChannel raised when the player's primary action charge is updated")]
        FloatEventChannelSO m_PrimaryActionChargeUpdateEventChannel;

        [SerializeField, Tooltip("Event channel which will be raised when Entity's Primary Action is executed")]
        private VoidEventChannelSO m_PrimaryActionExecutedEventChannel;

        [SerializeField] private TextMeshProUGUI m_PrimaryActionNameText;
        [SerializeField] private Slider m_ChargeSlider;

        private bool m_IsCharging;

        private void OnEnable()
        {
            m_PrimaryActionAssignedEventChannel.OnEventRaised += Init;
            m_PrimaryActionStartedEventChannel.OnEventRaised += StartCharging;
            m_PrimaryActionChargeUpdateEventChannel.OnEventRaised += UpdateChargeAmount;
            m_PrimaryActionExecutedEventChannel.OnEventRaised += OnActionExecuted;
        }

        private void OnDisable()
        {
            m_PrimaryActionAssignedEventChannel.OnEventRaised -= Init;
            m_PrimaryActionStartedEventChannel.OnEventRaised -= StartCharging;
            m_PrimaryActionChargeUpdateEventChannel.OnEventRaised -= UpdateChargeAmount;
            m_PrimaryActionExecutedEventChannel.OnEventRaised -= OnActionExecuted;
        }

        public void Init(TagSO tag)
        {
            m_PrimaryActionNameText.text = tag.name;
        }

        private void StartCharging()
        {
            m_IsCharging = true;
        }

        private void UpdateChargeAmount(float amount)
        {
            if (!m_IsCharging)
                return;

            m_ChargeSlider.value = Mathf.Clamp01(amount);
        }

        private void OnActionExecuted()
        {
            m_IsCharging = false;
        }
    }
}
