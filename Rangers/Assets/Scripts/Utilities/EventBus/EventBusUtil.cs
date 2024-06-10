using BTG.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BTG.Utilities.EventBus
{
    public static class EventBusUtil
    {
        /// <summary>
        /// Gets or sets the list of event types.
        /// </summary>
        public static IReadOnlyList<Type> EventTypes { get; set; }

        /// <summary>
        /// Gets or sets the list of event bus types.
        /// </summary>
        public static IReadOnlyList<Type> EventBusTypes { get; set; }

#if UNITY_EDITOR

        /// <summary>
        /// Gets or sets the current play mode state.
        /// </summary>
        public static PlayModeStateChange PlayModeState { get; set; }

        /// <summary>
        /// Initializes the editor by registering the play mode state change callback.
        /// </summary>
        [InitializeOnLoadMethod]
        public static void InitializeEditor()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        /// Callback function that is invoked when the play mode state changes.
        /// </summary>
        /// <param name="state">The new play mode state.</param>
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            PlayModeState = state;
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                ClearAllBuses();
            }
        }

#endif

        /// <summary>
        /// Initializes the EventBusUtil by setting the EventTypes and EventBusTypes properties.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IEvent));
            EventBusTypes = InitializeAllBuses();
        }

        /// <summary>
        /// Clears all the event buses.
        /// </summary>
        public static void ClearAllBuses()
        {
            Debug.Log("Clearing all buses");
            foreach (var busType in EventBusTypes)
            {
                var clearMethod = busType.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                clearMethod.Invoke(null, null);
            }
        }

        /// <summary>
        /// Initializes all the event buses and returns a list of the event bus types.
        /// </summary>
        /// <returns>A list of the event bus types.</returns>
        static List<Type> InitializeAllBuses()
        {
            List<Type> eventBusTypes = new List<Type>();

            var typedef = typeof(EventBus<>);
            foreach (var eventType in EventTypes)
            {
                var busType = typedef.MakeGenericType(eventType);
                eventBusTypes.Add(busType);
                Debug.Log($"Initialized EventBus<{busType.Name}>");
            }

            return eventBusTypes;
        }
    }
}
