using BTG.Tank.UltimateAction;
using System;
using System.Threading;
using System.Threading.Tasks;
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
        private CancellationTokenSource m_CancellationTokenSource;

        public Transform Transform => m_Controller.Transform;

        public event Action<float> OnUltimateExecuted;          // float is duration of the ultimate action

        public float Duration => m_UltimateAction.Duration;

        public TankUltimateController(TankController controller, IUltimateAction action)
        {
            m_CancellationTokenSource = new CancellationTokenSource();

            m_UltimateAction = action;
            m_Controller = controller;

            _ = AutoChargeUltimate(m_CancellationTokenSource.Token);
        }

        public void OnDestroy()
        {
            m_UltimateAction.OnDestroy();
        }

        public void ExecuteUltimateAction()
        {
            m_UltimateAction.TryExecute(this);
            OnUltimateExecuted?.Invoke(Duration);

            _ = AutoChargeUltimate(m_CancellationTokenSource.Token);
        }

        private async Task AutoChargeUltimate(CancellationToken token)
        {
            try
            {
                while (!m_UltimateAction.IsFullyCharged)
                {
                    m_UltimateAction.Charge(m_UltimateAction.ChargeRate);
                    await Task.Delay(1000, token);
                }
            }
            catch (TaskCanceledException)
            {
                // Task was cancelled
            }
        }
    }
}


