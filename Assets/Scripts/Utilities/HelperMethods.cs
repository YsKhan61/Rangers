using System.Threading;
using System.Threading.Tasks;

namespace BTG.Utilities
{
    /// <summary>
    /// Few methods here are used to help in some common tasks that are used in the project.
    /// </summary>
    public static class HelperMethods
    {
        /// <summary>
        /// This method is used to invoke the given action in the next frame.
        /// </summary>
        /// <param name="action">the action to invoke</param>
        /// <returns>the task</returns>
        public static async Task InvokeInNextFrame(System.Action action)
        {
            await Task.Yield();
            action?.Invoke();
        }

        /// <summary>
        /// This method is used to invoke the given action after the given seconds.
        /// </summary>
        /// <param name="seconds">seconds after which the action will be invoked</param>
        /// <param name="action">the action to invoke</param>
        /// <param name="token"> the cancellation token to cancel the task</param>
        /// <returns>the task</returns>
        public static async Task InvokeAfterAsync(int seconds, System.Action action, CancellationToken token)
        {
            try
            {
                await Task.Delay(seconds * 1000, token);
                action?.Invoke();
            }
            catch (TaskCanceledException)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// This method is used to dispose the given CancellationTokenSource.
        /// It cancels the token and then disposes it.
        /// It helps in avoiding memory leaks
        /// </summary>
        /// <param name="cancellationTokenSource"> the CancellationTokenSource to dispose</param>
        public static void DisposeCancellationTokenSource(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
        }
    }
}