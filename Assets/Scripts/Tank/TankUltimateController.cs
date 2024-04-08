using BTG.Tank.UltimateAction;
using BTG.Utilities;
using System;
using UnityEngine;

namespace BTG.Tank
{
    /// <summary>
    /// Controls the ultimate action of the tank.
    /// </summary>
    public class TankUltimateController
    {
        public Transform TankTransform => m_Brain.Transform;
        public Transform FirePoint => m_Brain.FirePoint;
        public IDamageable Damageable => m_Brain.Damageable;
        public LayerMask LayerMask => m_Brain.OppositionLayerMask;

        private IUltimateAction m_UltimateAction;
        private TankBrain m_Brain;

        public TankUltimateController(
            TankBrain brain, 
            UltimateActionFactorySO ultimateFactoryData)
        {
            m_Brain = brain;
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

        public void SubscribeToUltimateActionAssignedEvent(Action<string> action)
            => m_UltimateAction.OnUltimateActionAssigned += action;

        public void SubscribeToUltimateExecutedEvent(Action action) => 
            m_UltimateAction.OnUltimateActionExecuted += action;

        public void SubscribeToCameraShakeEvent(Action<float> action)
        {
            if (m_UltimateAction is ICameraShakeUltimateAction camShakeUltAction)
            {
                camShakeUltAction.OnExecuteCameraShake += action;
            }
        }

        public void SubscribeToChargeUpdatedEvent(Action<int> action) => 
            m_UltimateAction.OnChargeUpdated += action;

        public void SubscribeToFullyChargedEvent(Action action) =>
            m_UltimateAction.OnFullyCharged += action;
    }
}


