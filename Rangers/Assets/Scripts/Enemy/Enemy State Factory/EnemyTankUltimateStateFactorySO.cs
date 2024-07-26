using BTG.StateMachine;
using BTG.Utilities;
using UnityEngine;

namespace BTG.Enemy
{
    public abstract class EnemyTankUltimateStateFactorySO : ScriptableObject
    {
        [SerializeField, Tooltip("Tag of the Ultimate Action")]
        private TagSO m_UltimateActionTag;
        public TagSO UltimateActionTag => m_UltimateActionTag;

        public abstract IState CreateState(EnemyTankStateMachine owner);
    }
}
