using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using Valheim.CustomRaids.Configuration;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Utilities;

namespace Valheim.CustomRaids.Debug;

[HarmonyPatch(typeof(EnvMan), nameof(EnvMan.Awake))]
internal static class EvnManTest
{
    private static void Postfix(ref EnvMan __instance)
    {
        try
        {
            if (ConfigurationManager.GeneralConfig?.WriteEnvironmentDataToDisk?.Value == true)
            {
                string filePath = FileUtils.PrepareWriteDebugFile("environments.txt", "environments (aka. weather)");

                var fields = typeof(EnvSetup).GetFields();
                List<string> lines = new List<string>(__instance.m_environments.Count * fields.Length);

                foreach (var item in __instance.m_environments)
                {
                    foreach (var field in fields)
                    {
                        lines.Add($"{field.Name}: {field.GetValue(item)}");
                    }
                }
                File.WriteAllLines(filePath, lines);
            }
        }
        catch (Exception e)
        {
            Log.LogWarning("Error while attempting to write environments to file.", e);
        }
    }
}

[HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.Start))]
internal static class ZoneSystemPatch
{
    private static void Postfix(ref ZoneSystem __instance)
    {
        try
        {
            if (ConfigurationManager.GeneralConfig?.WriteGlobalKeyDataToDisk?.Value == true)
            {
                string filePath = FileUtils.PrepareWriteDebugFile("global_keys.txt", "global keys");
                File.WriteAllLines(filePath, __instance.GetGlobalKeys());
            }
        }
        catch (Exception e)
        {
            Log.LogWarning("Error while attempting to write global keys to file.", e);
        }
    }
}
