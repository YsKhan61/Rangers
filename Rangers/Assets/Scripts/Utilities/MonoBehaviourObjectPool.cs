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
        protected override T GetItem()
        {
            T item = base.GetItem();
            item.gameObject.SetActive(true);
            return item;
        }

        protected override void ReturnItem(T item)
        {
            item.gameObject.SetActive(false);
            base.ReturnItem(item);
        }
    }
}


