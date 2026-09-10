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

        var currentEnvSetup = LifecycleManager.State == GameState.Dedicated
            ? GetSimulateEnvironment(context.Position)
            : EnvMan.instance.GetCurrentEnvironment();

        var currentEnv = currentEnvSetup
            .m_name?
            .Trim()
            .ToUpperInvariant();

        return RequiredEnvironments.Any(x => x == currentEnv);
    }

    private static float GetHeight(Vector3 position) => 
        ZoneManager.GetZone(
            ZoneSystem.GetZone(position))
                .Height(position);

    private static EnvSetup GetSimulateEnvironment(Vector3 position)
    {
        // If environment is forced on server, grab that.
        if (EnvMan.instance && !string.IsNullOrWhiteSpace(EnvMan.instance.m_forceEnv))
        {
            return EnvMan.instance.GetEnv(EnvMan.instance.m_forceEnv);
        }

        // Simulate current environment.
        bool isAshlands = WorldGenerator.IsAshlands(position.x, position.z);
        bool isDeepNorth = WorldGenerator.IsDeepnorth(position.x, position.y);

        BiomeSector biome;

        if (isAshlands && GetHeight(position) <= EnvMan.instance.m_oceanLevelEnvCheckAshlandsDeepnorth)
        {
            biome = ZNet.World.m_biomeData.Biomes[Heightmap.Biome.AshLands].Sectors[0];
        }
        else if (isDeepNorth && GetHeight(position) <= EnvMan.instance.m_oceanLevelEnvCheckAshlandsDeepnorth)
        {
            biome = ZNet.World.m_biomeData.Biomes[Heightmap.Biome.DeepNorth].Sectors[0];
        }
        else
        {
            biome = WorldGenerator.instance.GetBiomeSector(position);
        }

        var potentialEnvs = EnvMan.instance.GetAvailableEnvironments(biome);

        // Pick env by seeded random. Kinda odd, but whatever, it's how Valheim does it.

        // TODO: Consider shifting time by -transition period, to fake the delayed change?
        var randomSeed = ((long)ZNet.instance.GetTimeSeconds()) / EnvMan.instance.m_environmentDuration;
        var existingRandomSeed = UnityEngine.Random.state;
        UnityEngine.Random.InitState((int)randomSeed);

        var currentEnv = EnvMan.instance.SelectWeightedEnvironment(potentialEnvs);

        // Some weird overrides going on here, but whatever. Valheim is Valheim.
        foreach (var envEntry in potentialEnvs)
        {
            if (isAshlands && envEntry.m_ashlandsOverride)
            {
                currentEnv = envEntry.m_env;
            }

            if (isDeepNorth && envEntry.m_deepnorthOverride)
            {
                currentEnv = envEntry.m_env;
            }
        }

        // Reset random to before our little random hacking.
        UnityEngine.Random.state = existingRandomSeed;

        return currentEnv;
    }
}
