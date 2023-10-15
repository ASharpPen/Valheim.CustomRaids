using System;
using HarmonyLib;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Patches;

/// <summary>
/// For some reason player globalkeys are added to a list for later update
/// when the key is first added.
/// This list is only properly added to the player upon Player.Awake, or Player.SetLocalPlayer.
/// This means raids and other global-key related conditions will be unable to react
/// correctly if the player is simply moving around, without triggering a reinstantation of the Player object.
/// 
/// This fix just forces the keys to get updated, right after a creature is killed.
/// </summary>
[HarmonyPatch]
internal static class Fix_PlayerKeys_Not_Updating_Patch
{
    [HarmonyPatch(typeof(Character), nameof(Character.OnDeath))]
    [HarmonyPostfix]
    private static void AddQueuedPlayerKeysOnCharacterDeath()
    {
        try
        {
            Player.m_localPlayer?.AddQueuedKeys();
        }
        catch (Exception e)
        {
            Log.LogError("Error while attempting to update queued player keys.", e);
        }
    }
}
