using BTG.Factory;


namespace BTG.Actions.PrimaryAction
{
    /// <summary>
    /// Any primary action of an actor should implement this interface.
    /// </summary>
    public interface IPrimaryAction : IFactoryItem
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
        /// Set the actor of the action
        /// </summary>
        public void SetActor(IPrimaryActor actor);

        /// <summary>
        /// Start executing the action. It will keep executing until it is stopped.
        /// </summary>
        public void StartAction();

        /// <summary>
        /// Stop executing the action or finishes the action.
        /// </summary>
        public void StopAction();

        /// <summary>
        /// This method is called to start the action and turn it off after certain time
        /// </summary>
        public void AutoStartStopAction(int stopTime);
    }
}