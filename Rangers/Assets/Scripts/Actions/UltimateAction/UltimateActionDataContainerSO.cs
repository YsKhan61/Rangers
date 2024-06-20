using BTG.Utilities;
using System;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// This scriptable object holds all the ultimate action data scriptable objects in the project.
    /// </summary>
    [CreateAssetMenu(fileName = "UltimateActionDataContainer", menuName = "ScriptableObjects/UltimateActionDataContainerSO")]
    public class UltimateActionDataContainerSO : GuidContainerSO<UltimateActionDataSO>
    {
        public UltimateActionDataSO GetUltimateActionDataByGuid(Guid guid)
        {
            if (!TryGetData(guid, out UltimateActionDataSO ultimateActionData))
            {
                Debug.LogError($"UltimateActionData with guid {guid} not found in {name}");
                return null;
            }
            return ultimateActionData;
        }

        public TagSO GetUltimateActionTagByGuid(Guid guid)
        {
            foreach (var ultimateActionData in DataList)
            {
                if (ultimateActionData.Tag.Guid == guid)
                {
                    return ultimateActionData.Tag;
                }
            }

            Debug.LogError($"UltimateActionTag with guid {guid} not found in {name}");
            return null;
        }
    }
}
