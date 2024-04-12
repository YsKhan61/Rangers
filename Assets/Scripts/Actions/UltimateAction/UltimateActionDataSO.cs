
using UnityEngine;

namespace BTG.Actions.UltimateAction
{
    public abstract class UltimateActionDataSO : ScriptableObject
    {
        [SerializeField, Tooltip("The amount of charging per second")] private float m_ChargeRate;
        public float ChargeRate => m_ChargeRate;
    }
}
