using BTG.Factory;
using BTG.Utilities;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// An interface for the ultimate action
    /// An ultimate action is a special ability that can be executed by the IUltimateActor
    /// </summary>
    public interface IUltimateAction : IDestroyable, IFactoryItem
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

        /// <summary>
        /// Set the actor of the ultimate action
        /// </summary>
        public void SetActor(IUltimateActor actor);

        /// <summary>
        /// Auto charge the ultimate action
        /// </summary>
        public void AutoCharge();

        /// <summary>
        /// Charge the ultimate action with a specific amount
        /// </summary>
        /// <param name="amount">charge amount to add</param>
        public void Charge(float amount);

        /// <summary>
        /// Try to execute the ultimate action
        /// </summary>
        /// <returns>true if success, false otherwise</returns>
        public bool TryExecute();
    }
}
