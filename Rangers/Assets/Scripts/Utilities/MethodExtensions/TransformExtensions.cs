

using UnityEngine;

namespace BTG.Utilities
{
    /// <summary>
    /// This class contains extension methods for the Transform class.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// This extension method is used to set the parent of the transform and set the local position and rotation.
        /// </summary>
        /// <returns></returns>
        public static Transform SetParent(this Transform transform, Transform parent, Vector3 localPosition, Quaternion localRotation)
        {
            transform.SetParent(parent);
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;

            return transform;
        }

        /// <summary>
        /// This extension method is used to set the position and rotation of the transform.
        /// </summary>
        public static Transform SetPose(this Transform transform, in Pose pose)
        {
            transform.position = pose.position;
            transform.rotation = pose.rotation;

            return transform;
        }
    }
}