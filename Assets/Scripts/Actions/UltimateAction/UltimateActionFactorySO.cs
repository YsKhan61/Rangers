using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// An abstract class for the ultimate action factory
    /// </summary>
    public abstract class UltimateActionFactorySO : ScriptableObject
    {
        [SerializeField, Tooltip("Tag for the ultimate action")]
        private TagSO m_UltimateTag;
        /// <summary>
        /// Tag for the ultimate action
        /// </summary>
        public TagSO UltimateTag => m_UltimateTag;

        /// <summary>
        /// Create the ultimate action
        /// </summary>
        /// <param name="actor">sets the actor of the ultimate</param>
        /// <returns>the created ultimate action</returns>
        public abstract IUltimateAction CreateUltimateAction(IUltimateActor actor);
    }
}
