using BTG.Utilities;
using System;
using UnityEngine;

namespace BTG.Actions.UltimateAction
{
    public interface IUltimateActor
    {
        public bool IsPlayer { get; }

        public event Action<float, float> OnPlayerCamShake;

        public Transform EntityTransform { get; }

        public LayerMask LayerMask { get; }

        public IDamageable Damageable { get; }

        public Transform FirePoint { get; }

        public void ToggleTankVisibility(bool value);
        public void ShakePlayerCamera(float amount, float duration);
    }
}