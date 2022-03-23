using HarmonyLib;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Patches;

[HarmonyPatch(typeof(RandEventSystem))]
internal static class RandEventSystem_StartRandomEvent_Patch
{
    /// <summary>
    /// Stop new raids from initializing when another event is currently active.
    /// </summary>
    [HarmonyPatch(nameof(RandEventSystem.StartRandomEvent))]
    [HarmonyPrefix]
    private static bool DontStartWhileActive()
    {
        if (RandEventSystem.instance.GetActiveEvent() is not null || RandEventSystem.instance.GetCurrentRandomEvent() is not null)
        {
            Log.LogTrace("Skipping starting of new raid due to already active event.");
            return false;
        }

        return true;
    }
}
