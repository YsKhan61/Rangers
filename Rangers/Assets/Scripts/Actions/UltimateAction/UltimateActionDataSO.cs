using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// This scriptable object holds the data for the ultimate action
    /// It contains the tag and the charge rate of the ultimate action as these are the common properties of all the ultimate actions
    /// Any ultimate action data scriptable object should inherit from this class
    /// Every ultimate action data scriptable object should have a unique tag.
    /// </summary>
    public abstract class UltimateActionDataSO : GuidSO
    {
        [SerializeField] 
        [Tooltip("Tag of the ultimate action. Every ultimate action data scriptable object should have a unique tag")] 
        private TagSO m_Tag;
        /// <summary>
        /// The tag of the ultimate action. Every ultimate action data scriptable object should have a unique tag
        /// </summary>
        public TagSO Tag => m_Tag;

        [SerializeField, Tooltip("Rate at which the ultimate will be charged")] 
        private float m_ChargeRate;
        /// <summary>
        /// Rate at which the ultimate will be charged
        /// </summary>
        public float ChargeRate => m_ChargeRate;
    }
}
