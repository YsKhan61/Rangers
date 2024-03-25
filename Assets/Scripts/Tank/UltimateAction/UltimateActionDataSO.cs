
using UnityEngine;

namespace BTG.Tank.UltimateAction
{
    public abstract class UltimateActionDataSO : ScriptableObject
    {
        [SerializeField, Tooltip("The amount of charging per second")] private float m_ChargeRate;
        public float ChargeRate => m_ChargeRate;
    }
}
