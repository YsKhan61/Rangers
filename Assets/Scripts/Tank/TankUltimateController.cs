using BTG.Tank.UltimateAction;
using UnityEngine;

namespace BTG.Tank
{
    public class TankUltimateController
    {
        private IUltimateAction m_UltimateAction;
        private TankController m_Controller;

        public Transform Transform => m_Controller.Transform;

        public TankUltimateController(TankController controller, IUltimateAction action)
        {
            m_UltimateAction = action;
            m_Controller = controller;
        }

        public void ExecuteUltimateAction()
        {
            m_UltimateAction.Execute(this);
        }
    }
}


