using System;
using UnityEngine;


namespace BTG.Gameplay.Configuration
{
    /// <summary>
    /// Data representation of a Character, containing such things as its starting HP and Mana, and what attacks it can do.
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/ConfigurationData/CharacterClassSO")]
    [Serializable]
    public sealed class CharacterClassSO : ScriptableObject
    {
        /*[Tooltip("which character this data represents")]
        public CharacterTypeEnum CharacterType;*/
    }
}