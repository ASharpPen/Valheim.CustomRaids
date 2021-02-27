using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Valheim.CustomRaids
{
    [HarmonyPatch(typeof(EnvMan), "Awake")]
    public static class EvnManTest
    {
        private static void Postfix(ref EnvMan __instance)
        {
            WriteToFile(__instance.m_environments, "env_man_environments");
        }

        public static void WriteToFile(object obj, string name)
        {
            string filePath = Path.GetFullPath($@".\{name}.txt");

            var fields = obj.GetType().GetFields();
            List<string> lines = new List<string>(fields.Length);
            foreach (var field in fields)
            {
                lines.Add($"{field.Name}: {field.GetValue(obj)}");
            }
            File.WriteAllLines(filePath, lines);
        }

        public static void WriteToFile<T>(List<T> list, string name)
        {

            string filePath = Path.GetFullPath($@".\{name}.txt");

            var fields = typeof(T).GetFields();
            List<string> lines = new List<string>(list.Count * fields.Length);

            foreach (var item in list)
            {
                foreach (var field in fields)
                {
                    lines.Add($"{field.Name}: {field.GetValue(item)}");
                }
            }
            File.WriteAllLines(filePath, lines);
        }
    }

    [HarmonyPatch(typeof(ZoneSystem), "Start")]
    public static class ZoneSystemPatch
    {
        private static void Postfix(ref ZoneSystem __instance)
        {
            string filePath = Path.GetFullPath($@".\ZoneSystem.txt");
            File.WriteAllLines(filePath, __instance.GetGlobalKeys());
        }

    }
}
