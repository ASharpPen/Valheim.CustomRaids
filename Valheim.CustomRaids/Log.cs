using System;
using UnityEngine;
using Valheim.CustomRaids.ConfigurationTypes;

namespace Valheim.CustomRaids
{
    public static class Log
    {
        private static string PluginName => "Custom Raids";

        public static void LogDebug(string message)
        {
            if (ConfigurationManager.DebugOn)
            {
                Debug.Log($"[{PluginName}]: {message}");
            }
        }

        public static void LogTrace(string message)
        {
            if (ConfigurationManager.GeneralConfig?.TraceLogging?.Value == true)
            {
                Debug.Log($"[{PluginName}]: {message}");
            }
        }

        public static void LogInfo(string message) => Debug.Log($"[{PluginName}]: {message}");

        public static void LogWarning(string message) => Debug.LogWarning($"[{PluginName}]: {message}");

        public static void LogError(string message, Exception e = null) => Debug.LogError($"[{PluginName}]: {message}; {e?.Message ?? ""}");
    }
}
