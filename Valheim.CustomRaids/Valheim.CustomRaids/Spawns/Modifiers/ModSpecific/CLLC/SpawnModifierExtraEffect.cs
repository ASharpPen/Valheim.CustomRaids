using CreatureLevelControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Core.Cache;
using Valheim.CustomRaids.Core.Configuration;

namespace Valheim.CustomRaids.Spawns.Modifiers.ModSpecific.CLLC
{
    internal class SpawnModifierExtraEffect : ISpawnModifier
    {
        private static SpawnModifierExtraEffect _instance;

        public static SpawnModifierExtraEffect Instance
        {
            get
            {
                return _instance ??= new SpawnModifierExtraEffect();
            }
        }

        public void Modify(SpawnContext context)
        {
            if (!context.Spawn || context.Spawn is null || context.Config is null)
            {
                return;
            }

            if (context.Config.TryGet(SpawnConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is SpawnConfigCLLC config && config.SetExtraEffect.Value.Length > 0)
                {
                    var character = ComponentCache.GetComponent<Character>(context.Spawn);

                    if (character is null)
                    {
                        return;
                    }

                    if (Enum.TryParse(config.SetExtraEffect.Value, true, out CreatureExtraEffect extraEffect))
                    {
                        Log.LogTrace($"Setting extra effect {extraEffect} for {context.Spawn.name}.");
                        CreatureLevelControl.API.SetExtraEffectCreature(character, extraEffect);
                    }
                }
            }
        }
    }
}
