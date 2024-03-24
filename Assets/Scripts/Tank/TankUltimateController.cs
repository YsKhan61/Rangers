using BTG.Tank.UltimateAction;
using System;
using System.Threading;
using UnityEngine;

namespace BTG.Tank
{
    /// <summary>
    /// Controls the ultimate action of the tank.
    /// </summary>
    public class TankUltimateController
    {
        private IUltimateAction m_UltimateAction;
        private TankController m_Controller;

        public Transform Transform => m_Controller.Transform;

        public float Duration => m_UltimateAction.Duration;

        public TankUltimateController(TankController controller, IUltimateAction action)
        {
            m_UltimateAction = action;
            m_Controller = controller;

            m_UltimateAction.AutoCharge();
        }

        public void OnDestroy()
        {
            m_UltimateAction.OnDestroy();
        }

        public void ExecuteUltimateAction()
        {
            m_UltimateAction.TryExecute(this);
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
            m_UltimateAction.OnExecuteCameraShake += action;
        }

        public void SubscribeToChargeUpdatedEvent(Action<int> action)
        {
            m_UltimateAction.OnChargeUpdated += action;
        }

        public void SubscribeToFullyChargedEvent(Action action)
        {
            m_UltimateAction.OnFullyCharged += action;
        }

        public void ShowTankView()
        {
            m_Controller.ShowView();
        }

        public void HideTankView()
        {
            m_Controller.HideView();
        }
    }
}


