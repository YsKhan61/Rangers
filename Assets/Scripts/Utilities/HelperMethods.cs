using System.Threading;
using System.Threading.Tasks;

namespace BTG.Utilities
{
    /// <summary>
    /// Few methods here are used to help in some common tasks that are used in the project.
    /// </summary>
    public static class HelperMethods
    {
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
    }
}