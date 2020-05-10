using UnityEngine;
using UnityEditor;
namespace Filterartifact
{
    public class DebugLog : LogBase
    {
        public static void LogInGMOrder(object message)
        {
#if GMORDER
            Debug.Log(message);
#endif
        }
    }
}