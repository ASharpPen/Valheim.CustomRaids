using CreatureLevelControl;
using System;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Core.Cache;
using Valheim.CustomRaids.Core.Configuration;

namespace Valheim.CustomRaids.Spawns.Modifiers.ModSpecific.CLLC
{
    internal class SpawnModifierInfusion : ISpawnModifier
    {
        private static SpawnModifierInfusion _instance;

        public static SpawnModifierInfusion Instance
        {
            get
            {
                return _instance ??= new SpawnModifierInfusion();
            }
        }

        public void Modify(SpawnContext context)
        {
            if (!context.Spawn || context.Spawn is null)
            {
                return;
            }

            if (context.Config.TryGet(SpawnConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is SpawnConfigCLLC config && config.SetInfusion.Value.Length > 0)
                {
                    var character = ComponentCache.GetComponent<Character>(context.Spawn);

                    if (character is null)
                    {
                        return;
                    }

                    if (Enum.TryParse(config.SetInfusion.Value, true, out CreatureInfusion infusion))
                    {
                        Log.LogTrace($"Setting infusion {infusion} for {context.Spawn.name}.");
                        CreatureLevelControl.API.SetInfusionCreature(character, infusion);
                    }
                }
            }
        }
    }
}
