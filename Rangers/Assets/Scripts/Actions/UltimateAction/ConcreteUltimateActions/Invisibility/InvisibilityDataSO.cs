using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    [CreateAssetMenu(fileName = "Invisibility", menuName = "ScriptableObjects/UltimateAction/InvisibilityDataSO")]
    public class InvisibilityDataSO : UltimateActionDataSO
    {
        [SerializeField, Tooltip("Execution duration")] 
        private int m_Duration;
        public int Duration => m_Duration;

        [SerializeField, Tooltip("The tag of the effect that executes before the actor will become invisible")] 
        private TagSO m_InvisibleEffectTag;
        public TagSO InvisibleEffectTag => m_InvisibleEffectTag;

        [SerializeField, Tooltip("The tag of the effect that executes just before the actor will become visible")] 
        private TagSO m_VisibleEffectTag;
        public TagSO VisibleEffectTag => m_VisibleEffectTag;

        [SerializeField, Tooltip("This is the duration of the visibility particle effect, after which we will active the gameobject")] 
        private int m_VisibleDelay;
        public int VisibleDelay => m_VisibleDelay;
    }
}
