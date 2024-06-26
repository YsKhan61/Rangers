using UnityEngine;


namespace BTG.Utilities
{
    /// <summary>
    /// A MonoBehaviour Object Pool Class with basic functionality,
    /// which can be inherited to implement object pools for any type of MonoBehaviour objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoBehaviourObjectPool<T> : GenericObjectPool<T> where T : MonoBehaviour
    {
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

        protected override T GetItem()
        {
            if (Container == null)
            {
                CreateContainerAndClearPool();
            }

            T item = base.GetItem();
            item.gameObject.SetActive(true);
            return item;
        }

        protected override void ReturnItem(T item)
        {
            item.transform.SetParent(Container);
            item.gameObject.SetActive(false);
            base.ReturnItem(item);
        }

        private void CreateContainerAndClearPool()
        {
            // means the Container is destroyed by scene change or it was never created
            // so create a new container and clear the pooled items as some game object of pooled items might be destroyed
            m_Container = new GameObject("Container of " + typeof(T).Name).transform;
            m_PooledItems.Clear();
        }
    }
}


