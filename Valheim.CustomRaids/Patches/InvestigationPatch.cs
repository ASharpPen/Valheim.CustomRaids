using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using Valheim.CustomRaids.Configuration;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids
{
    [HarmonyPatch(typeof(EnvMan), "Awake")]
	public static class EvnManTest
	{
		private static void Postfix(ref EnvMan __instance)
		{
			if (ConfigurationManager.GeneralConfig.WriteEnvironmentDataToDisk.Value)
			{
				string filePath = Path.Combine(Paths.PluginPath, "env_man_environments.txt");
				Log.LogInfo($"Writing global keys to {filePath}");

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
	}

	[HarmonyPatch(typeof(ZoneSystem), "Start")]
	public static class ZoneSystemPatch
	{
		private static void Postfix(ref ZoneSystem __instance)
		{
			if (ConfigurationManager.GeneralConfig.WriteGlobalKeyDataToDisk.Value)
			{
				string filePath = Path.Combine(Paths.PluginPath, "ZoneSystem.txt");
				Log.LogInfo($"Writing global keys to {filePath}");
				File.WriteAllLines(filePath, __instance.GetGlobalKeys());
			}
		}
	}
}
