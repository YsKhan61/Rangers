using BTG.Utilities;
using System;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// This scriptable object holds all the ultimate action data scriptable objects in the project.
    /// </summary>
    [CreateAssetMenu(fileName = "UltimateActionDataContainer", menuName = "ScriptableObjects/UltimateAction/UltimateActionDataContainerSO")]
    public class UltimateActionDataContainerSO : GuidContainerSO<UltimateActionDataSO>
    {
        public bool TryGetUltimateActionDataByGuid(Guid guid, out UltimateActionDataSO ultimateActionData)
        {
            if (!TryGetData(guid, out ultimateActionData))
            {
                Debug.LogError($"UltimateActionData with guid {guid} not found in {name}");
                return false;
            }
            return true;
        }

        public bool TryGetUltimateActionTagByGuid(Guid guid, out TagSO tag)
        {
            foreach (var ultimateActionData in DataList)
            {
                if (ultimateActionData.Tag.Guid == guid)
                {
                    tag = ultimateActionData.Tag;
                    return true;
                }
            }
            tag = null;
            Debug.Assert(false, $"UltimateActionTag with guid {guid} not found in {name}");
            return false;
        }
    }
}
