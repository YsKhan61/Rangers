using BTG.Tank.UltimateAction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BTG.UI
{
    public class UltimateUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_UltimateNameText;
        [SerializeField] private TextMeshProUGUI m_ChargedAmountText;
        [SerializeField] private Image m_UnreadyImage;
        [SerializeField] private Image m_ReadyImage;

        private void Start()
        {
            m_ChargedAmountText.text = "0";
            m_UnreadyImage.gameObject.SetActive(true);
            m_ReadyImage.gameObject.SetActive(false);
        }

        public void AssignUltimateActionName(string name)
        {
            m_UltimateNameText.text = name;
        }

        public void UpdateChargeAmount(int amount)
        {
            m_ChargedAmountText.text = amount.ToString();
        }

        public void OnFullyCharged(IUltimateAction _)
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
