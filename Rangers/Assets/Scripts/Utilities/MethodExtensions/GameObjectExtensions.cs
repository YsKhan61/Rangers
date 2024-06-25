using Unity.Netcode;
using Unity.Netcode.Components;
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

        /*public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }*/


        /// <summary>
        /// This extension method is used to create a network object for the game object.
        /// It also adds a network rigidbody if the game object has a rigidbody.
        /// </summary>
        public static GameObject CreateNetworkObject(this GameObject gameObject)
        {
            gameObject.GetOrAddComponent<NetworkObject>();
            if (gameObject.TryGetComponent(out Rigidbody _))
            {
                gameObject.GetOrAddComponent<NetworkRigidbody>();
            }

            return gameObject;
        }
        
        

        /// <summary>
        /// This extension method is used to create a network transform for the game object.
        /// It uses the given settings to set the component fields.
        /// </summary>
        public static GameObject CreateNetworkTransform(this GameObject gameObject, NetworkTransformSettings settings)
        {
            NetworkTransform nt = (NetworkTransform)gameObject.GetOrAddComponent<NetworkTransform>();

            nt.SyncPositionX = settings.SyncPositionX;
            nt.SyncPositionY = settings.SyncPositionY;
            nt.SyncPositionZ = settings.SyncPositionZ;

            nt.SyncRotAngleX = settings.SyncRotAngleX;
            nt.SyncRotAngleY = settings.SyncRotAngleY;
            nt.SyncRotAngleZ = settings.SyncRotAngleZ;
            
            nt.SyncScaleX = settings.SyncScaleX;
            nt.SyncScaleY = settings.SyncScaleY;
            nt.SyncScaleZ = settings.SyncScaleZ;

            return gameObject;
        }
    }

    /// <summary>
    /// This struct contains the settings for the network transform component.
    /// </summary>
    public struct NetworkTransformSettings
    {
        public bool SyncPositionX;
        public bool SyncPositionY;
        public bool SyncPositionZ;

        public bool SyncRotAngleX;
        public bool SyncRotAngleY;
        public bool SyncRotAngleZ;

        public bool SyncScaleX;
        public bool SyncScaleY;
        public bool SyncScaleZ;
    }
}