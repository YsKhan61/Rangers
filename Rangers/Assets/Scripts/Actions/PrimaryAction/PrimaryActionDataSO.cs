using BTG.Utilities;
using UnityEngine;

namespace BTG.Actions.PrimaryAction
{
    public abstract class PrimaryActionDataSO : GuidSO
    {
        [SerializeField]
        [Tooltip("Tag of the primary action. Every primary action data scriptable object should have a unique tag")]
        private TagSO m_Tag;
        /// <summary>
        /// The tag of the primary action. Every primary action data scriptable object should have a unique tag
        /// </summary>
        public TagSO Tag => m_Tag;

        [SerializeField, Tooltip("Damage done by the projectile")]
        int m_Damage;
        public int Damage => m_Damage;

        [SerializeField]
        int m_ChargeTime;
        public int ChargeTime => m_ChargeTime;
    }
}