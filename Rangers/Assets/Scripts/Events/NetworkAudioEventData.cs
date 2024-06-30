using BTG.Utilities;
using BTG.Utilities.EventBus;
using Unity.Netcode;
using UnityEngine;

namespace BTG.Events
{
    public struct NetworkAudioEventData : INetworkSerializable, IEvent
    {
        /// <summary>
        /// Is the audio event only for the owner client
        /// </summary>
        public bool OwnerClientOnly;

        /// <summary>
        /// The client id of the owner, if the audio event is only for the owner client
        /// </summary>
        public ulong OwnerClientId;

        /// <summary>
        /// Whether the effect should follow a network object.
        /// </summary>
        public bool FollowNetworkObject;

        /// <summary>
        /// Id of the network object to follow.
        /// Only needed if FollowNetworkObject is true.
        /// </summary>
        public ulong FollowNetowrkObjectId;

        /// <summary>
        /// Tag of the audio data
        /// </summary>
        public NetworkGuid AudioTagNetworkGuid;
        
        /// <summary>
        /// Position where the audio clip will be played
        /// </summary>
        public Vector3 Position;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref OwnerClientOnly);
            serializer.SerializeValue(ref FollowNetworkObject);
            serializer.SerializeValue(ref FollowNetowrkObjectId);
            serializer.SerializeValue(ref OwnerClientId);
            serializer.SerializeValue(ref AudioTagNetworkGuid);
            serializer.SerializeValue(ref Position);
        }

        public NetworkAudioEventData(
            bool ownerClientOnly,
            ulong ownerClientId,

            bool followNetworkObject,
            ulong followNetowrkObjectId,

            NetworkGuid audioTagNetworkGuid, 
            Vector3 position)
        {
            OwnerClientOnly = ownerClientOnly;
            OwnerClientId = ownerClientId;

            FollowNetworkObject = followNetworkObject;
            FollowNetowrkObjectId = followNetowrkObjectId;

            AudioTagNetworkGuid = audioTagNetworkGuid;
            Position = position;
        }
    }
}