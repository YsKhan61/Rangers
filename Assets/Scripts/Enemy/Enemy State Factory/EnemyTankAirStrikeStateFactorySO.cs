using BTG.StateMachine;
using UnityEngine;

namespace BTG.Enemy
{
    [CreateAssetMenu(fileName = "EnemyTankAirStrikeStateFactory", menuName = "ScriptableObjects/Factory/EnemyStateFactory/EnemyTankAirStrikeStateFactorySO")]
    public class EnemyTankAirStrikeStateFactorySO : EnemyTankUltimateStateFactorySO
    {
        public override IState CreateState(EnemyTankStateMachine owner)
        {
            return new EnemyTankAirStrikeState(owner);
        }
    }
}
