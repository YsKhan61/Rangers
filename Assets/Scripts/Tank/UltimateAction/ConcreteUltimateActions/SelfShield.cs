using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class SelfShield : IUltimateAction
    {
        public float Duration => 0f; // not implemented

        public void Execute(TankUltimateController controller)
        {
            Debug.Log("Ultimate: Self Shield executed");
        }

        public void OnDestroy()
        {
            Debug.Log("Ultimate: Self Shield destroyed");
        }
    }
}
