using System.Threading;
using System.Threading.Tasks;

namespace BTG.Utilities
{
    /// <summary>
    /// Few methods here are used to help in some common tasks that are used in the project.
    /// </summary>
    public static class HelperMethods
    {
        public static async void InvokeAfterAsync(int seconds, System.Action action, CancellationToken token)
        {
            await Task.Delay(seconds * 1000, token);
            action?.Invoke();
        }
    }
}