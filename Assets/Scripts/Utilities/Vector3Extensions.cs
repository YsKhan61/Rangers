using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BTG.Utilities
{
    public static class Vector3Extensions
    {
        public static Vector3 SetYOffset(this Vector3 vector, float y = 0)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        public static Vector3 SetOffset(this Vector3 vector, float x = 0, float y = 0, float z = 0)
        {
            return vector + new Vector3(x, y, z);
        }
    }
}

