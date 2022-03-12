using HarmonyLib;

namespace Valheim.CustomRaids.Spawns.Patches
{
    [HarmonyPatch(typeof(Character))]
    public static class CharacterSetFactionPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void AssignFaction(Character __instance, ZNetView ___m_nview)
        {
            var zdo = ___m_nview?.GetZDO();
            if (zdo is null)
            {
                return;
            }

            var faction = zdo.GetInt("faction", -1);

            if (faction >= 0)
            {
                __instance.m_faction = (Character.Faction)faction;
            }
        }
    }
}
