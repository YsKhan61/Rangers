using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

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

        /// <summary>
        /// This method is used to copy the properties of the source component to the target component.
        /// </summary>
        public static void CopyComponentProperties(Component sourceComponent, Component targetComponent)
        {
            // Get the type of the source and target components
            System.Type type = sourceComponent.GetType();

            // Get all properties of the component
            PropertyInfo[] properties = type.GetProperties();

            // Iterate through each property and copy its value
            foreach (PropertyInfo property in properties)
            {
                // Check if the property can be read and written
                if (property.CanRead && property.CanWrite)
                {
                    // Get the value of the property from the source component
                    object value = property.GetValue(sourceComponent, null);

                    // Set the value of the property in the target component
                    property.SetValue(targetComponent, value, null);
                }
            }
        }
    }
}