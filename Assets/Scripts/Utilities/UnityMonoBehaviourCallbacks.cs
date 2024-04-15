using System.Collections.Generic;
using UnityEngine;

namespace BTG.Utilities
{
    public interface IStartable
    {
        public void Start();
    }

    public interface IFixedUpdatable
    { 
        public void FixedUpdate();
    }

    public interface IUpdatable
    {
        public void Update();
    }

    public interface IDestroyable
    {
        public void OnDestroy();
    }

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
                destroyables[i].OnDestroy();
            }
        }

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