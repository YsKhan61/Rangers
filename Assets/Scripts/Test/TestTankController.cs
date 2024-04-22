using BTG.Tank;
using UnityEngine;


namespace BTG.Test
{
    public class TestTankController : MonoBehaviour
    {
        [SerializeField]
        TankView m_TankView;

        [ContextMenu("FullChargeUltimate")]
        public void FullChargeUltimate()
        {
            m_TankView.TankBrain.ChargeUltimate(100);           // 100 is the full charge amount
        }
    }

}
