using BTG.Utilities;
using System;
using UnityEngine;

namespace BTG.Actions.PrimaryAction
{
    [CreateAssetMenu(fileName = "PrimaryActionDataContainer", menuName = "ScriptableObjects/PrimaryAction/PrimaryActionDataContainerSO")]
    public class PrimaryActionDataContainerSO : GuidContainerSO<PrimaryActionDataSO>
    {
        public bool TryGetPrimaryActionDataByGuid(Guid guid, out PrimaryActionDataSO primaryActionData)
        {
            if (!TryGetData(guid, out primaryActionData))
            {
                Debug.LogError($"PrimaryActionData with guid {guid} not found in {name}");
                return false;
            }
            return true;
        }

        public bool TryGetPrimaryActionTagByGuid(Guid guid, out TagSO tag)
        {
            foreach (var primaryActionData in DataList)
            {
                if (primaryActionData.Tag.Guid == guid)
                {
                    tag = primaryActionData.Tag;
                    return true;
                }
            }
            tag = null;
            Debug.Assert(false, $"UltimateActionTag with guid {guid} not found in {name}");
            return false;
        }

        public bool TryGetPrimaryActionDataByTag(TagSO tag, out PrimaryActionDataSO primaryActionData)
        {
            foreach (var data in DataList)
            {
                if (data.Tag == tag)
                {
                    primaryActionData = data;
                    return true;
                }
            }
            primaryActionData = null;
            Debug.LogError($"PrimaryActionData with tag {tag} not found in {name}");
            return false;
        }
    }
}
