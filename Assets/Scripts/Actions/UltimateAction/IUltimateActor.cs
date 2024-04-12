using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    public interface IUltimateActor
    {
        public bool IsPlayer { get; }

        public Transform Transform { get; }

        public LayerMask OppositionLayerMask { get; }

        public IDamageable Damageable { get; }

        public Transform FirePoint { get; }
        public void TryExecuteUltimate();

        public void ToggleActorVisibility(bool value);
        public void ShakePlayerCamera(float amount, float duration);
    }
}