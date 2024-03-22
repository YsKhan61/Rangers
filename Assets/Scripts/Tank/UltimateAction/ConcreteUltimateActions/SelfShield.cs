using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class SelfShield : IUltimateAction
    {
        public void Execute(TankUltimateController controller)
        {
            Debug.Log("Ultimate: Self Shield executed");
        }
    }
}
