using BTG.Tank.UltimateAction;
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
        private TankController m_Controller;

        public Transform Transform => m_Controller.Transform;

        public event Action<float> OnUltimateExecuted;          // float is duration of the ultimate action

        public float Duration => m_UltimateAction.Duration;

        public TankUltimateController(TankController controller, IUltimateAction action)
        {
            m_UltimateAction = action;
            m_Controller = controller;
        }

        public void OnDestroy()
        {
            m_UltimateAction.OnDestroy();
        }

        public void ExecuteUltimateAction()
        {
            m_UltimateAction.Execute(this);
            OnUltimateExecuted?.Invoke(Duration);
        }
    }
}


