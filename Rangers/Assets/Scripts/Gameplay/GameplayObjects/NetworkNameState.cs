using BTG.Utilities;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace BTG.Gameplay.GameplayObjects
{
    /// <summary>
    /// NetworkBehaviour containing only one NetworkVariableString which represents this object's name.
    /// </summary>
    public class NetworkNameState : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkVariable<FixedPlayerName> Name = new();
    }
}