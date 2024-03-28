using BTG.Tank.UltimateAction;

namespace BTG.Enemy
{
    public class EnemyAI
    {
        public void OnUltimateFullyCharged(IUltimateAction ultimate)
        {
            ultimate.TryExecute();
        }
    }
}
