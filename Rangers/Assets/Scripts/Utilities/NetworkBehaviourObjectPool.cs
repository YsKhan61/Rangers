using Unity.Netcode;


namespace BTG.Utilities
{
    /// <summary>
    /// A NetworkBehaviour Object Pool Class with basic functionality, 
    /// which can be inherited to implement object pools for any type of NetworkBehaviour objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NetworkBehaviourObjectPool<T> : MonoBehaviourObjectPool<T> where T : NetworkBehaviour
    {
        protected override T GetItem()
        {
            T item = base.GetItem();
            item.GetComponent<NetworkObject>().Spawn();
            return item;
        }

        protected override void ReturnItem(T item)
        {
            item.GetComponent<NetworkObject>().Despawn(true);
            base.ReturnItem(item);
        }
    }
}


