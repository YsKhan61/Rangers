
using UnityEngine;

namespace BTG.Tank.UltimateAction
{
    public abstract class UltimateActionDataSO : ScriptableObject
    {
        [SerializeField] private float m_Duration;
        public float Duration => m_Duration;

        [SerializeField, Tooltip("The amount of charging per second")] private float m_ChargeRate;
        public float ChargeRate => m_ChargeRate;
    }
}
