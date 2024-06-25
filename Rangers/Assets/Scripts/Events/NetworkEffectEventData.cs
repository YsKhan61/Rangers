using BTG.Utilities;
using BTG.Utilities.EventBus;
using Unity.Netcode;
using UnityEngine;

namespace BTG.Events
{
    /// <summary>
    /// In multiplayer - an event data struct that holds the data for an effect event.
    /// </summary>
    public struct NetworkEffectEventData : INetworkSerializable, IEvent
    {
        /// <summary>
        /// Whether the effect should be visible only to the owner client.
        /// </summary>
        public bool OwnerClientOnly;
        
        /// <summary>
        /// Id of the owner client.
        /// </summary>
        public ulong OwnerClientId;
        
        /// <summary>
        /// Whether the effect should follow a network object.
        /// </summary>
        public bool FollowNetworkObject;
        
        /// <summary>
        /// Id of the network object to follow.
        /// </summary>
        public ulong FollowNetowrkObjectId;
        
        /// <summary>
        /// The tag of the effect converted to a network guid.
        /// </summary>
        public NetworkGuid EffectTagNetworkGuid;
        
        /// <summary>
        /// Position of the effect to spawn.
        /// </summary>
        public Vector3 EffectPosition;

        /// <summary>
        /// Duration of the effect.
        /// </summary>
        public int Duration;

        // Serialize the data
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref OwnerClientOnly);
            serializer.SerializeValue(ref FollowNetworkObject);
            serializer.SerializeValue(ref FollowNetowrkObjectId);
            serializer.SerializeValue(ref OwnerClientId);
            serializer.SerializeValue(ref EffectTagNetworkGuid);
            serializer.SerializeValue(ref EffectPosition);
            serializer.SerializeValue(ref Duration);
        }

        public NetworkEffectEventData(
            bool ownerClientOnly,
            ulong ownerClientId,

            bool followNetworkObject,
            ulong followNetworkObjectId,
            
            TagSO effectTag, 
            Vector3 effectPosition, 
            int duration)
        {
            OwnerClientOnly = ownerClientOnly;
            OwnerClientId = ownerClientId;

            FollowNetworkObject = followNetworkObject;
            FollowNetowrkObjectId = followNetworkObjectId;
            
            EffectTagNetworkGuid = effectTag.Guid.ToNetworkGuid();
            EffectPosition = effectPosition;
            Duration = duration;
        }
    }
}