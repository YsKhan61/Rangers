using System.Collections.Generic;
using UnityEngine;


namespace BTG.Utilities
{
    /// <summary>
    /// This is a Generic Object Pool Class with basic functionality, which can be inherited to implement object pools for any type of objects.
    /// </summary>
    public abstract class GenericObjectPool<T> where T : class
    {
        /// <summary>
        /// List of pooled items
        /// </summary>
        private List<PooledItem<T>> m_PooledItems = new List<PooledItem<T>>();

        private Transform m_Container;
        /// <summary>
        /// The game object, under which all the pooled items will be parented when not in use
        /// </summary>
        public Transform Container
        {
            get
            {
                if (m_Container == null)
                {
                    CreateContainerAndClearPool();
                }
                return m_Container;
            }
        }

        /// <summary>
        /// Get an item from the pool
        /// </summary>
        protected T GetItem()
        {
            if (Container == null)      
            {
                CreateContainerAndClearPool();
            }

            if (m_PooledItems.Count > 0)
            {
                PooledItem<T> item = m_PooledItems.Find(item => !item.isUsed);
                if (item != null)
                {
                    item.isUsed = true;
                    return item.Item;
                }
            }
            return CreateNewPooledItem();
        }

        /// <summary>
        /// Create a new item and add it to the pool
        /// </summary>
        private T CreateNewPooledItem()
        {
            PooledItem<T> newItem = new PooledItem<T>();
            newItem.Item = CreateItem();
            newItem.isUsed = true;
            m_PooledItems.Add(newItem);
            return newItem.Item;
        }

        private void CreateContainerAndClearPool()
        {
            // means the Container is destroyed by scene change or it was never created
            // so create a new container and clear the pooled items as some game object of pooled items might be destroyed
            m_Container = new GameObject("Container of " + typeof(T).Name).transform;
            m_PooledItems.Clear();
        }

        /// <summary>
        /// Create an item of type T
        /// </summary>
        protected abstract T CreateItem();

        /// <summary>
        /// Return an item to the pool
        /// </summary>
        /// <param name="item"></param>
        protected void ReturnItem(T item)
        {
            if (item == null)
            {
                Debug.Assert(false, "Trying to return a null item to the pool");
                return;
            }

            PooledItem<T> pooledItem = m_PooledItems.Find(i => i.Item.Equals(item));
            pooledItem.isUsed = false;
        }

        /// <summary>
        /// A class to hold the pooled item and it's usage status
        /// </summary>
        /// <typeparam name="U"></typeparam>
        public class PooledItem<U>
        {
            public U Item;
            public bool isUsed;
        }
    }
}


