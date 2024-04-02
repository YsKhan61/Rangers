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
        private IUltimateAction m_UltimateAction;
        public IUltimateAction UltimateAction => m_UltimateAction;

        private TankBrain m_Controller;
        public TankBrain TankController => m_Controller;

        public Transform TankTransform => m_Controller.Transform;

        public Transform FirePoint => m_Controller.FirePoint;

        public IDamageable Damageable => m_Controller.Damageable;
        public LayerMask LayerMask => m_Controller.OppositionLayerMask;

        public TankUltimateController(
            TankBrain controller, 
            UltimateActionFactorySO ultimateFactoryData
            )
        {
            m_Controller = controller;
            m_UltimateAction = ultimateFactoryData.CreateUltimateAction(this);
        }

        public void OnDestroy()
        {
            m_UltimateAction.OnDestroy();
        }

        public void EnableUltimate()
        {
            m_UltimateAction.Enable();
        }

        public void DisableUltimate()
        {
            m_UltimateAction.Disable();
        }

        public void SubscribeToUltimateActionAssignedEvent(Action<string> action)
        {
            m_UltimateAction.OnUltimateActionAssigned += action;
        }

        public void SubscribeToUltimateExecutedEvent(Action action)
        {
            m_UltimateAction.OnUltimateActionExecuted += action;
        }

        public void SubscribeToCameraShakeEvent(Action<float> action)
        {
            if (m_UltimateAction is ICameraShakeUltimateAction camShakeUltAction)
            {
                camShakeUltAction.OnExecuteCameraShake += action;
            }
        }

        public void SubscribeToChargeUpdatedEvent(Action<int> action)
        {
            m_UltimateAction.OnChargeUpdated += action;
        }

        public void SubscribeToFullyChargedEvent(Action<IUltimateAction> action)
        {
            m_UltimateAction.OnFullyCharged += action;
        }
    }
}


