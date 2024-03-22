using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class AutoTarget : IUltimateAction
    {
        public float Duration => 0f; // not implemented

        public void Execute(TankUltimateController controller)
        {
            Debug.Log("Ultimate: Auto Target executed");
        }

        public void OnDestroy()
        {
            Debug.Log("Ultimate: Auto Target destroyed");
        }
    }
}
