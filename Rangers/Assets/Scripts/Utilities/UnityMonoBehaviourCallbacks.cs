using System.Collections.Generic;
using UnityEngine;

namespace BTG.Utilities
{
    /// <summary>
    /// An interface for objects that need to be started when the game starts.
    /// It is used to avoid using the Start method in the MonoBehaviour.
    /// </summary>
    public interface IStartable
    {
        /// <summary>
        /// This method will be called when the UnityMonoBehaviourCallback Start method is called.
        /// </summary>
        public void Start();
    }

    /// <summary>
    /// An interface for objects that need to be updated in the FixedUpdate method.
    /// It is used to avoid using the FixedUpdate method in the MonoBehaviour.
    /// </summary>
    public interface IFixedUpdatable
    {
        /// <summary>
        /// This method will be called when the UnityMonoBehaviourCallback FixedUpdate method is called.
        /// </summary>
        public void FixedUpdate();
    }

    /// <summary>
    /// An interface for objects that need to be updated in the Update method.
    /// It is used to avoid using the Update method in the MonoBehaviour.
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// This method will be called when the UnityMonoBehaviourCallback Update method is called.
        /// </summary>
        public void Update();
    }

    /// <summary>
    /// An interface for objects that need to be destroyed when the game object is destroyed.
    /// It is used to avoid using the OnDestroy method in the MonoBehaviour.
    /// </summary>
    public interface IDestroyable
    {
        /// <summary>
        /// This method will be called when the UnityMonoBehaviourCallback OnDestroy method is called.
        /// </summary>
        public void Destroy();
    }

    /// <summary>
    /// This class is a singleton that holds the Unity MonoBehaviour callbacks.
    /// It is used to provide a way to register and unregister objects to the Unity MonoBehaviour callbacks.
    /// </summary>
    public class UnityMonoBehaviourCallbacks : Singleton<UnityMonoBehaviourCallbacks>
    {
        private List<IStartable> startables = new List<IStartable>();
        private List<IFixedUpdatable> fixedUpdatables = new List<IFixedUpdatable>();
        private List<IUpdatable> updatables = new List<IUpdatable>();
        private List<IDestroyable> destroyables = new List<IDestroyable>();


        private void Start()
        {
            for (int i = 0; i < startables.Count; i++)
            {
                startables[i].Start();
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < fixedUpdatables.Count; i++)
            {
                fixedUpdatables[i].FixedUpdate();
            }
        }

        private void Update()
        {
            for (int i = 0; i < updatables.Count; i++)
            {
                updatables[i].Update();
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < destroyables.Count; i++)
            {
                destroyables[i].Destroy();
            }
        }

        /// <summary>
        /// Registers an object to the Unity Start method.
        /// </summary>
        public void RegisterToStart(IStartable startable)
        {
            if (!startables.Contains(startable))
            {
                startables.Add(startable);
            }
            else
            {
                Debug.Log("Trying to register an already registered startable");
            }
        }

        /// <summary>
        /// Registers an object to the Unity Update method.
        /// </summary>
        public void RegisterToUpdate(IUpdatable updatable)
        {
            if (!updatables.Contains(updatable))
            {
                updatables.Add(updatable);
            }
            else
            {
                Debug.Log("Trying to register an already registered updatable");
            }
        }

        /// <summary>
        /// Registers an object to the Unity FixedUpdate method.
        /// </summary>
        public void RegisterToFixedUpdate(IFixedUpdatable fixedUpdatable)
        {
            if (!fixedUpdatables.Contains(fixedUpdatable))
            {
                fixedUpdatables.Add(fixedUpdatable);
            }
            else
            {
                Debug.Log("Trying to register an already registered fixed updatable");
            }
        }

        /// <summary>
        /// Registers an object to the Unity OnDestroy method.
        /// </summary>
        public void RegisterToDestroy(IDestroyable destroyable)
        {
            if (!destroyables.Contains(destroyable))
            {
                destroyables.Add(destroyable);
            }
            else
            {
                Debug.Log("Trying to register an already registered destroyable");
            }
        }

        /// <summary>
        /// Unregisters an object from the Unity Start method.
        /// </summary>
        public void UnregisterFromStart(IStartable startable)
        {
            if (startables.Contains(startable))
            {
                startables.Remove(startable);
            }
            else
            {
                Debug.Log("Trying to unregister an unregistered startable");
            }
        }

        /// <summary>
        /// unregister an object from the Unity Update method.
        /// </summary>
        public void UnregisterFromUpdate(IUpdatable updatable)
        {
            if (updatables.Contains(updatable))
            {
                updatables.Remove(updatable);
            }
            else
            {
                Debug.Log("Trying to unregister an unregistered updatable");
            }
        }

        /// <summary>
        /// Unregisters an object from the Unity FixedUpdate method.
        /// </summary>
        public void UnregisterFromFixedUpdate(IFixedUpdatable fixedUpdatable)
        {
            if (fixedUpdatables.Contains(fixedUpdatable))
            {
                fixedUpdatables.Remove(fixedUpdatable);
            }
            else
            {
                Debug.Log("Trying to unregister an unregistered fixed updatable");
            }
        }

        /// <summary>
        /// Unregisters an object from the Unity OnDestroy method.
        /// </summary>
        public void UnregisterFromDestroy(IDestroyable destroyable)
        {
            if (destroyables.Contains(destroyable))
            {
                destroyables.Remove(destroyable);
            }
            else
            {
                Debug.Log("Trying to unregister an unregistered destroyable");
            }
        }
    }
}