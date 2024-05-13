using BTG.Utilities;

namespace BTG.Actions.PrimaryAction
{
    /// <summary>
    /// Any primary action of an actor should implement this interface.
    /// </summary>
    public interface IPrimaryAction
    {
        public event System.Action OnPrimaryActionExecuted;

        /// <summary>
        /// Enable the action.
        /// </summary>
        public void Enable();

        /// <summary>
        /// Disable the action.
        /// </summary>
        public void Disable();

        /// <summary>
        /// Start executing the action. It will keep executing until it is stopped.
        /// </summary>
        public void StartAction();

        /// <summary>
        /// Stop executing the action or finishes the action.
        /// </summary>
        public void StopAction();
    }
}