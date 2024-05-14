
using BTG.Utilities;
using BTG.Utilities.DI;

namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// An interface for the ultimate action
    /// An ultimate action is a special ability that can be executed by the IUltimateActor
    /// </summary>
    public interface IUltimateAction : IDestroyable
    {
        public enum State
        {
            Disabled,
            Charging,
            FullyCharged,
            Executing
        }

        /// <summary>
        /// Invoked after the ultimate action is assigned. 
        /// It contains the tag of the ultimate action
        /// </summary>
        public event System.Action<TagSO> OnUltimateActionAssigned;

        /// <summary>
        /// Invoked when the charge is updated.
        /// It contains the charged amount
        /// </summary>
        public event System.Action<int> OnChargeUpdated;

        /// <summary>
        /// Invoked when the ultimate action is fully charged
        /// </summary>
        public event System.Action OnFullyCharged;

        /// <summary>
        /// Invoked when the ultimate action is executed
        /// </summary>
        public event System.Action OnUltimateActionExecuted;

        /// <summary>
        /// Get the tag of the ultimate action
        /// </summary>
        public TagSO Tag { get; }

        /// <summary>
        /// The actor that can execute the ultimate action
        /// </summary>
        public IUltimateActor Actor { get; }

        /// <summary>
        /// Enable the ultimate action
        /// </summary>
        public void Enable();

        /// <summary>
        /// Disable the ultimate action
        /// </summary>
        public void Disable();

        public void AutoCharge();

        public void Charge(float amount);

        public bool TryExecute();
    }
}
