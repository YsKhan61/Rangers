using UnityEngine;
using UnityEngine.UI;

namespace BTG.Entity.Tank
{
    public class TankUI : MonoBehaviour
    {
        [SerializeField]
        Slider m_ChargeSlider;

        public void UpdateChargedAmountUI(float value)
        {
            m_ChargeSlider.value = value;
        }
    }
}

