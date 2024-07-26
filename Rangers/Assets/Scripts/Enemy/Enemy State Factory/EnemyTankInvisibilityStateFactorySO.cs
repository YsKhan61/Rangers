using BTG.StateMachine;
using UnityEngine;

namespace BTG.Enemy
{
    [CreateAssetMenu(fileName = "EnemyTankInvisibilityStateFactory", menuName = "ScriptableObjects/Factory/EnemyStateFactory/EnemyTankInvisibilityStateFactorySO")]
    public class EnemyTankInvisibilityStateFactorySO : EnemyTankUltimateStateFactorySO
    {
        public override IState CreateState(EnemyTankStateMachine owner)
        {
            return new EnemyTankInvisibilityState(owner);
        }
    }
}
