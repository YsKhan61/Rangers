using System.Collections.Generic;


namespace BTG.Utilities
{
    /// <summary>
    /// This is a Generic Object Pool Class with basic functionality, which can be inherited to implement object pools for any type of objects.
    /// </summary>
    /// <typeparam object Type to be pooled = "T"></typeparam>
    public abstract class GenericObjectPool<T> where T : class
    {
        /// <summary>
        /// List of pooled items
        /// </summary>
        private List<PooledItem<T>> pooledItems = new List<PooledItem<T>>();

        /// <summary>
        /// Get an item from the pool
        /// </summary>
        protected T GetItem()
        {
            if (pooledItems.Count > 0)
            {
                PooledItem<T> item = pooledItems.Find(item => !item.isUsed);
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
            pooledItems.Add(newItem);
            return newItem.Item;
        }

        /// <summary>
        /// Create an item of type T
        /// </summary>
        protected abstract T CreateItem();

        /// <summary>
        /// Return an item to the pool
        /// </summary>
        /// <param name="item"></param>
        public void ReturnItem(T item)
        {
            PooledItem<T> pooledItem = pooledItems.Find(i => i.Item.Equals(item));
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


