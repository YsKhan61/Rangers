using UnityEngine;


namespace BTG.Utilities
{
    [CreateAssetMenu(fileName = "IntData", menuName = "ScriptableObjects/DataContainers/IntDataSO")]
    public class IntDataSO : ScriptableObject
    {
        public int Value;

        // Create implicit conversion from IntDataSO to int
        public static implicit operator int(IntDataSO intDataSO) => intDataSO.Value;
    }

}