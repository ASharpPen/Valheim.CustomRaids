using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.CustomRaids.Resetter;
using Valheim.CustomRaids.World;

namespace Valheim.CustomRaids.Raids.Conditions;

internal class ConditionEnvironment : IRaidCondition
{
    private List<string> _requiredEnvironments;

    public List<string> RequiredEnvironments 
    { 
        get { return _requiredEnvironments; }
        set 
        { 
            _requiredEnvironments = value
                .Select(x => x.Trim().ToUpperInvariant())
                .ToList(); 
        }
    }

    public ConditionEnvironment(List<string> requiredEnvironments)
    {
        RequiredEnvironments = requiredEnvironments;
    }

    public bool IsValid(RaidContext context)
    {
        if (RequiredEnvironments is null || RequiredEnvironments.Count == 0)
        {
            return true;
        }

        var currentEnv = GetCurrent(context.Position)?
            .m_name?
            .Trim()?
            .ToUpperInvariant();

        return RequiredEnvironments.Any(x => x == currentEnv);
    }

    public static EnvSetup GetCurrent(Vector3 position)
{
        if (WorldStartupResetPatch.State == GameState.Dedicated)
        {
            // If environment is forced on server, grab that.
            if (EnvMan.instance && !string.IsNullOrWhiteSpace(EnvMan.instance.m_forceEnv))
            {
                return EnvMan.instance.GetEnv(EnvMan.instance.m_forceEnv);
            }

            // Simulate current environment.
            var biome = ZoneManager.GetZone(ZoneSystem.instance.GetZone(position)).Biome;

            var potentialEnvs = EnvMan.instance.GetAvailableEnvironments(biome);

            // Pick env by seeded random. Kinda odd, but whatever, it's how Valheim does it.

            // TODO: Consider shifting time by -transition period, to fake the delayed change?
            var randomSeed = ((long)ZNet.instance.GetTimeSeconds()) / EnvMan.instance.m_environmentDuration;
            var existingRandomSeed = UnityEngine.Random.state;
            UnityEngine.Random.InitState((int)randomSeed);

            var currentEnv = EnvMan.instance.SelectWeightedEnvironment(potentialEnvs);

            // Reset random to before our little random hacking.
            UnityEngine.Random.state = existingRandomSeed;

            return currentEnv;
        }
        else
        {
            return EnvMan.instance.GetCurrentEnvironment();
        }
    }
}
