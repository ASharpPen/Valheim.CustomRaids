using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core.Cache;
using Valheim.CustomRaids.Core.Configuration;

namespace Valheim.CustomRaids.Spawns.Modifiers.ModSpecific.CLLC
{
    internal class SpawnModifierSetLevel : ISpawnModifier
    {
        private static SpawnModifierSetLevel _instance;

        public static SpawnModifierSetLevel Instance
        {
            get
            {
                return _instance ??= new SpawnModifierSetLevel();
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
                if (modConfig is SpawnConfigCLLC config && config.UseDefaultLevels.Value)
                {
                    var character = ComponentCache.GetComponent<Character>(context.Spawn);

                    if (character is null)
                    {
                        return;
                    }

                    var level = context.Config.MinLevel.Value;

                    for (int i = 0; i < context.Config.MaxLevel.Value - context.Config.MinLevel.Value; ++i)
                    {
                        if (UnityEngine.Random.Range(0, 100) > 10)
                        {
                            break;
                        }

                        ++level;
                    }

                    character.SetLevel(level);
                }
            }
        }
    }
}
