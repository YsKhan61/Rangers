using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class Invisibility : IUltimateAction
    {
        public float Duration => 0f; // not implemented

        public void Execute(TankUltimateController controller)
        {
            Debug.Log("Ultimate: Invisibility executed");
        }

        public void OnDestroy()
        {
            Debug.Log("Ultimate: Invisibility destroyed");
        }
    }
}
