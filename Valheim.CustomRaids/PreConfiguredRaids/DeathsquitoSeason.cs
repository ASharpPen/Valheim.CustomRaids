using BepInEx;
using System.IO;
using UnityEngine;

namespace Valheim.CustomRaids.PreConfiguredRaids
{
    public class DeathsquitoSeason
    {
        private const string Filename = "custom_raids.supplemental.deathsquitoseason.cfg";

        public void CreateConfigIfMissing()
        {
            string configPath = Path.Combine(Paths.ConfigPath, Filename);
            if (!File.Exists(configPath))
            {
                if (ConfigurationManager.DebugOn) Debug.Log($"Generating supplemental raid {configPath}");

                File.WriteAllText(configPath, FileDump);
            }
        }

        private static string FileDump =>
            @"
[DeathsquitoSeason]
Name = DeathsquitoSeason
Biome=513
Duration=3600
NearBaseOnly=true
StartMessage=It's the season
EndMessage=The mosquito season has ended
Enabled=false
RequiredGlobalKeys=defeated_bonemass

[DeathsquitoSeason.0]
Name=Deathsquito
Enabled=True
PrefabName=Deathsquito
MaxSpawned=3
SpawnInterval=60
SpawnDistance=0
SpawnRadiusMin=0
SpawnRadiusMax=1
GroupSizeMin=1
GroupSizeMax=1
SpawnAtNight=True
SpawnAtDay=True
HuntPlayer=True
GroundOffset=5
MaxLevel=1
MinLevel=1
";
    }
}
