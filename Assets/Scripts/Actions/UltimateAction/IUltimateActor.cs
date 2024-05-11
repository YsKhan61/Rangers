using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    public interface IUltimateActor : ITransform
    { 
        public bool IsPlayer { get; }
        public Transform FirePoint { get; }
        public LayerMask OppositionLayerMask { get; }
        public IUltimateAction UltimateAction { get; }
        public IDamageable Damageable { get; }

        public void TryExecuteUltimate();
        public void ToggleActorVisibility(bool value);
    }
}