using BTG.Utilities;
using System;
using UnityEngine;

namespace BTG.Entity
{
    public interface IEntityUltimateController
    {
        public event Action<float, float> OnPlayerCamShake;

        public Transform EntityTransform { get; }

        public LayerMask LayerMask { get; }

        public IDamageable Damageable { get; }

        public Transform FirePoint { get; }

        public void ToggleTankVisibility(bool value);
        public void ShakePlayerCamera(float amount, float duration);
    }
}