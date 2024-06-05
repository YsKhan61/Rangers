using BTG.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace BTG.Gameplay.UI
{
    /// <summary>
    /// This class is responsible for updating the ultimate UI of player
    /// </summary>
    public class UltimateUI : MonoBehaviour
    {
        [SerializeField, Tooltip("EventChannel raised when the player's ultimate is assigned")]
        TagEventChannelSO m_UltimateAssignedEventChannel;

        [SerializeField, Tooltip("EventChannel raised when the player's ultimate charge is updated")]
        IntEventChannelSO m_UltimateChargeUpdateEventChannel;

        [SerializeField, Tooltip("EventChannel raised when the player's ultimate is fully charged")]
        VoidEventChannelSO m_UltimateFullyChargedEventChannel;

        [SerializeField, Tooltip("EventChannel raised when the player's ultimate is executed")]
        VoidEventChannelSO m_UltimateExecutedEventChannel;


        [SerializeField] private TextMeshProUGUI m_UltimateNameText;
        [SerializeField] private TextMeshProUGUI m_ChargedAmountText;
        [SerializeField] private Image m_UnreadyImage;
        [SerializeField] private Image m_ReadyImage;

        private void OnEnable()
        {
            m_UltimateAssignedEventChannel.OnEventRaised += Init;
            m_UltimateChargeUpdateEventChannel.OnEventRaised += UpdateChargeAmount;
            m_UltimateFullyChargedEventChannel.OnEventRaised += OnFullyCharged;
            m_UltimateExecutedEventChannel.OnEventRaised += OnUltimateExecuted;
        }

        public void Init(TagSO tag)
        {
            m_UltimateNameText.text = tag.name;

            m_ChargedAmountText.text = "0";
            m_UnreadyImage.gameObject.SetActive(true);
            m_ReadyImage.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            m_UltimateAssignedEventChannel.OnEventRaised -= Init;
            m_UltimateChargeUpdateEventChannel.OnEventRaised -= UpdateChargeAmount;
            m_UltimateFullyChargedEventChannel.OnEventRaised -= OnFullyCharged;
            m_UltimateExecutedEventChannel.OnEventRaised -= OnUltimateExecuted;
        }

        public void UpdateChargeAmount(int amount)
        {
            m_ChargedAmountText.text = amount.ToString();
        }

        public void OnFullyCharged()
        {
            m_UnreadyImage.gameObject.SetActive(false);
            m_ReadyImage.gameObject.SetActive(true);
        }

        public void OnUltimateExecuted()
        {
            m_ReadyImage.gameObject.SetActive(false);
            m_UnreadyImage.gameObject.SetActive(true);
        }
    }
}
