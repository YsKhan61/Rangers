using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class AutoTarget : IUltimateAction
    {
        public void Execute(TankUltimateController controller)
        {
            Debug.Log("Ultimate: Auto Target executed");
        }
    }
}
