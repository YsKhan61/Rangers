using BTG.Entity;
using BTG.Tank.UltimateAction;
using BTG.Utilities;
using System;
using UnityEngine;

namespace BTG.Tank
{
    /// <summary>
    /// Controls the ultimate action of the tank.
    /// </summary>
    public class TankUltimateController : IEntityUltimateController
    {
        public bool IsPlayer { get; set; }

        public event Action<float, float> OnPlayerCamShake;

        public Transform EntityTransform => m_Brain.Transform;
        public Transform FirePoint => m_Brain.FirePoint;
        public IDamageable Damageable => m_Brain.Damageable;
        public LayerMask LayerMask => m_Brain.OppositionLayerMask;

        private IEntityUltimateAbility m_UltimateAction;
        public IEntityUltimateAbility UltimateAction => m_UltimateAction;

        private TankBrain m_Brain;

        public TankUltimateController(
            UltimateActionFactorySO ultimateFactoryData)
        {
            m_UltimateAction = ultimateFactoryData.CreateUltimateAction(this);
        }

        public void OnDestroy()
        {
            m_UltimateAction.OnDestroy();
        }

        public void EnableUltimate() => m_UltimateAction.Enable();

        public void DisableUltimate() => m_UltimateAction.Disable();

        public void TryExecuteUltimate() => m_UltimateAction?.TryExecute();

        public void ToggleTankVisibility(bool isVisible) => m_Brain.ToggleTankVisibility(isVisible);

        public void ShakePlayerCamera(float amount, float duration)
        {
            // Implement camera shake
            if (!IsPlayer) return;

            OnPlayerCamShake?.Invoke(amount, duration);
        }

        public void SubscribeToFullyChargedEvent(Action action) =>
            m_UltimateAction.OnFullyCharged += action;
    }
}


