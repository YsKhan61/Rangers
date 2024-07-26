using BTG.StateMachine;
using UnityEngine;

namespace BTG.Enemy
{
    [CreateAssetMenu(fileName = "EnemyTankSelfShieldStateFactory", menuName = "ScriptableObjects/Factory/EnemyStateFactory/EnemyTankSelfShieldStateFactorySO")]
    public class EnemyTankSelfShieldStateFactorySO : EnemyTankUltimateStateFactorySO
    {
        public override IState CreateState(EnemyTankStateMachine owner)
        {
            return new EnemyTankSelfShieldState(owner);
        }
    }
}
