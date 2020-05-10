using UnityEngine;
using UnityEditor;
using System;

namespace Filterartifact
{
    public class ILog
    {
#if GMORDER
        public static bool m_bLogError = true;
#else
        public static bool m_bLogError = false;
#endif
        public static bool m_bLog = false;
        public static bool m_bLogWarning = false;
        public static bool m_bLogNet = false;
        public static bool m_bLogException = false;

        public static void Log(object message)
        {
            if (m_bLog)
            {
                Debug.Log(message);
            }
        }

        public static void Log(object message, UnityEngine.Object context)
        {
            if (m_bLog)
            {
                Debug.Log(message, context);
            }
        }

        public static void LogException(Exception e)
        {
            if (m_bLogException)
            {
                Debug.LogException(e);
            }
        }

        public static void LogError(object message)
        {
            if (m_bLogError)
            {
                if (message == null)
                    Debug.LogError("null reference!");
                else
                    Debug.LogError(message);
            }
        }

        public static void LogError(object message, UnityEngine.Object context)
        {
            if (m_bLogError)
            {
                Debug.LogError(message, context);
            }
        }

        public static void LogWarning(object message)
        {
            if (m_bLogWarning)
            {
                Debug.LogWarning(message);
            }
        }

        public static void LogWarning(object message, UnityEngine.Object context)
        {
            if (m_bLogWarning)
            {
                Debug.LogWarning(message, context);
            }

        }
    }
}