using CreatureLevelControl;
using System;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Core.Configuration;
using Valheim.CustomRaids.Raids2.Configs.BepInEx;
using Valheim.CustomRaids.Spawns.Caches;

namespace Valheim.CustomRaids.Spawns.Modifiers.ModSpecific.CLLC
{
    internal class SpawnModifierBossAffix : ISpawnModifier
    {
        private static SpawnModifierBossAffix _instance;

        public static SpawnModifierBossAffix Instance
        {
            get
            {
                return _instance ??= new SpawnModifierBossAffix();
            }
        }

        public void Modify(SpawnContext context)
        {
            if (context.Spawn is null)
            {
                return;
            }

            if (context.Config.TryGet(SpawnConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is SpawnConfigCLLC config && config.SetBossAffix.Value.Length > 0)
                {
                    var character = SpawnCache.GetCharacter(context.Spawn);

                    if (character is null)
                    {
                        return;
                    }

                    if (!character.IsBoss())
                    {
                        return;
                    }

                    if (Enum.TryParse(config.SetBossAffix.Value, true, out BossAffix bossAffix))
                    {
                        Log.LogTrace($"Setting boss affix {bossAffix} for {context.Spawn.name}.");
                        CreatureLevelControl.API.SetAffixBoss(character, bossAffix);
                    }
                }
            }
        }
    }
}
