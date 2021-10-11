using BepInEx.Logging;
using System;
using Valheim.CustomRaids.Configuration;

namespace Valheim.CustomRaids.Core
{
    internal class Log
    {
        internal static ManualLogSource Logger;

        public static void LogDebug(string message)
        {
            if (ConfigurationManager.GeneralConfig?.DebugOn?.Value == true)
            {
                Logger.LogInfo($"{message}");
            }
        }

        public static void LogTrace(string message)
        {
            if (ConfigurationManager.GeneralConfig?.TraceLogging?.Value == true)
            {
                Logger.LogDebug($"{message}");
            }
        }

        public static void LogInfo(string message) => Logger.LogMessage($"{message}");

        public static void LogWarning(string message) => Logger.LogWarning($"{message}");

        public static void LogWarning(string message, Exception e) => Logger.LogWarning($"{message}\n{e}");

        public static void LogError(string message) => Logger.LogError($"{message}");

        public static void LogError(string message, Exception e) => Logger.LogError($"{message}\n{e}");
    }
}
