using System;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Core.Cache;

namespace Valheim.CustomRaids.Spawns.Modifiers.General;

public class SpawnModifierSetFaction : ISpawnModifier
{
    private static SpawnModifierSetFaction _instance;

    public static SpawnModifierSetFaction Instance
    {
        get
        {
            return _instance ??= new SpawnModifierSetFaction();
        }
    }

    public void Modify(SpawnContext context)
    {
        if (context.Spawn is null)
        {
            return;
        }

        var character = ComponentCache.GetComponent<Character>(context.Spawn);

        if (character is null)
        {
            return;
        }

        string factionName = null;

        if (!string.IsNullOrWhiteSpace(context.Config.Faction.Value))
        {
            factionName = context.Config.Faction.Value;
        }
        else if (!string.IsNullOrWhiteSpace(context.RaidConfig.Faction.Value))
        {
            factionName = context.RaidConfig.Faction.Value;
        }

        Character.Faction creatureFaction = Character.Faction.Boss;

        if (!string.IsNullOrWhiteSpace(factionName))
        {
            if (!Enum.TryParse(factionName.Trim(), true, out creatureFaction))
            {
                Log.LogWarning($"Failed to parse faction '{factionName}', defaulting to Boss.");
            }
        }

#if DEBUG
        Log.LogDebug($"Setting faction {creatureFaction}");
#endif
        character.m_faction = creatureFaction;
        ZdoCache.GetZdo(context.Spawn).Set("faction", (int)creatureFaction);
    }
}
