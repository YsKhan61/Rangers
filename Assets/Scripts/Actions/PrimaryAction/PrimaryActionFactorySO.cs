using BTG.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BTG.Actions.PrimaryAction
{
    /// <summary>
    /// An abstract class for the primary action factory
    /// </summary>
    public abstract class PrimaryActionFactorySO : ScriptableObject
    {
        [SerializeField, Tooltip("Tag for the primary action")]
        private TagSO m_PrimaryTag;

        /// <summary>
        /// Tag for the primary action
        /// </summary>
        public TagSO PrimaryTag => m_PrimaryTag;

        /// <summary>
        /// Create the primary action
        /// </summary>
        /// <param name="actor"> sets the actor of the primary action</param>
        public abstract IPrimaryAction CreatePrimaryAction(IPrimaryActor actor);
    }
}