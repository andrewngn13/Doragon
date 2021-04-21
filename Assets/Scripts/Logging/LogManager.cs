using UnityEngine;

// Own static loggger manager
namespace Doragon.Logging
{
    public static class DLogger
    {
        public static void Log(string content)
        {
            Debug.Log(content);
        }
        public static void LogWarning(string content)
        {
            Debug.LogWarning(content);
        }
        public static void LogError(string content)
        {
            Debug.LogError(content);
        }
    }
}