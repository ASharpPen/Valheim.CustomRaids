using BepInEx;
using HarmonyLib;
using System.IO;
using UnityEngine;
using Valheim.CustomRaids.Core;

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
                Log.LogDebug($"Generating supplemental raid {configPath}");

                File.WriteAllText(configPath, FileDump);
            }
        }

        private static string FileDump =>
            @"
[DeathsquitoSeason]
Name = DeathsquitoSeason
Duration=3600
NearBaseOnly=true
StartMessage=Deathsquito season
EndMessage=The season has ended
Enabled=false
RequiredGlobalKeys=defeated_bonemass

[DeathsquitoSeason.0]
Name=Deathsquito
Enabled=True
PrefabName=Deathsquito
MaxSpawned=10
SpawnInterval=10
SpawnDistance=0
SpawnRadiusMin=0
SpawnRadiusMax=1
GroupSizeMin=3
GroupSizeMax=3
SpawnAtNight=True
SpawnAtDay=True
HuntPlayer=True
GroundOffset=5
MaxLevel=1
MinLevel=1
";
    }
}
