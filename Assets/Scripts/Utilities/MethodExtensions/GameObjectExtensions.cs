

using System;
using UnityEngine;

namespace BTG.Utilities
{
    /// <summary>
    /// This class contains extension methods for the GameObject class.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// This extension method is used to get or add a component of the given type to the game object.
        /// </summary>
        /// <typeparam name="T">type of the component</typeparam>
        /// <param name="gameObject">gameobject of the component</param>
        /// <returns>the component</returns>
        public static Component GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out var component))
            {
                return component;
            }

            return gameObject.AddComponent<T>();
        }
    }
}