using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class Invisibility : IUltimateAction
    {
        public void Execute(TankUltimateController controller)
        {
            Debug.Log("Ultimate: Invisibility executed");
        }
    }
}
