using HarmonyLib;
using Valheim.CustomRaids.Configuration;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Patches;

[HarmonyPatch(typeof(RandEventSystem))]
internal static class RandEventSystem_PauseTimer_Patch
{
    [HarmonyPatch(nameof(RandEventSystem.UpdateRandomEvent))]
    [HarmonyPrefix]
    public static bool PauseTimerWhilePlayersAreOffline()
    {
        if (!ZNet.instance.IsServer())
        {
            return true;
        }

        if (ConfigurationManager.GeneralConfig.PauseEventTimersWhileOffline.Value &&
            ZNet.instance.GetAllCharacterZDOS().Count == 0)
        {
            return false;
        }

        return true;
    }
}
