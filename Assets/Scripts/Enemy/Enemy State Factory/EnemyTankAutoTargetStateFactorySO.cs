using BTG.StateMachine;
using UnityEngine;

namespace BTG.Enemy
{
    [CreateAssetMenu(fileName = "EnemyTankAutoTargetStateFactory", menuName = "ScriptableObjects/Factory/EnemyStateFactory/EnemyTankAutoTargetStateFactorySO")]
    public class EnemyTankAutoTargetStateFactorySO : EnemyTankUltimateStateFactorySO
    {
        public override IState CreateState(EnemyTankStateMachine owner)
        {
            return new EnemyTankAutoTargetState(owner);
        }
    }
}
