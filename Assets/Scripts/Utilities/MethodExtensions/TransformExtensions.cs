

using UnityEngine;

namespace BTG.Utilities
{
    public static class TransformExtensions
    {
        public static Transform SetParent(
            this Transform transform, 
            Transform parent,
            in Vector3 localPosition, 
            in Quaternion localRotation)
        {
            transform.SetParent(parent);
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;

            return transform;
        }

        public static Transform SetPose(this Transform transform, in Pose pose)
        {
            transform.position = pose.position;
            transform.rotation = pose.rotation;

            return transform;
        }
    }
}