using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids.Serverside.Schedulers
{
    [HarmonyPatch(typeof(RandEventSystem))]
    internal class Patch_RandEventSystem_SaveLoad
    {
        [HarmonyPatch(nameof(RandEventSystem.PrepareSave))]
        [HarmonyPostfix]
        private static void OnPrepareSave()
        {

        }

        [HarmonyPatch(nameof(RandEventSystem.Load))]
        [HarmonyPostfix]
        private static void OnLoad()
        {

        }
    }
}
