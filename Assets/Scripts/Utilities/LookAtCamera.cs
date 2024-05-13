using UnityEngine;

namespace BTG.Utilities
{
    /// <summary>
    /// LookAtCamera is a utility script that makes the object it is attached to always look at the camera.
    /// </summary>
    public class LookAtCamera : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
