using UnityEngine;


namespace BTG.Utilities
{
    public static class ColorDebug
    {
        public static void LogInRed(string message)
        {
            Debug.Log("<color=red>" + message + "</color>");
        }

        public static void LogInGreen(string message)
        {
            Debug.Log("<color=green>" + message + "</color>");
        }
    }
}

