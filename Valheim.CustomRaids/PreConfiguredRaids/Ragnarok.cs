using BepInEx;
using System.IO;
using UnityEngine;

namespace Valheim.CustomRaids.PreConfiguredRaids
{
    public class Ragnarok
    {
        private const string Filename = "custom_raids.supplemental.ragnarok.cfg";

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
[Ragnarok]
Name = Ragnarok
Biome=513
Duration=300
NearBaseOnly=true
PauseIfNoPlayerInArea=True
ForceEnvironment=ThunderStorm
ForceMusic=CombatEventL2
StartMessage=Ragnarök has come! The endtimes have begun
EndMessage=The war has settled... for a while
Enabled=false
RequiredGlobalKeys=defeated_dragon

[Ragnarok.0]
Name=Eikthyr
Enabled=True
PrefabName=Eikthyr
MaxSpawned=1
SpawnInterval=600
SpawnDistance=0
SpawnRadiusMin=0
SpawnRadiusMax=1
GroupSizeMin=1
GroupSizeMax=1
SpawnAtNight=True
SpawnAtDay=True
HuntPlayer=True
GroundOffset=0.5
MaxLevel=1
MinLevel=1

[Ragnarok.1]
Name=ElderBark
Enabled=True
PrefabName=ElderBark
MaxSpawned=1
SpawnInterval=600
SpawnDistance=0
SpawnRadiusMin=0
SpawnRadiusMax=1
GroupSizeMin=1
GroupSizeMax=1
SpawnAtNight=True
SpawnAtDay=True
HuntPlayer=True
GroundOffset=0.5
MaxLevel=1
MinLevel=1

[Ragnarok.2]
Name=Bonemass
Enabled=True
PrefabName=Bonemass
MaxSpawned=1
SpawnInterval=600
SpawnDistance=0
SpawnRadiusMin=0
SpawnRadiusMax=1
GroupSizeMin=1
GroupSizeMax=1
SpawnAtNight=True
SpawnAtDay=True
HuntPlayer=True
GroundOffset=0.5
MaxLevel=1
MinLevel=1

[Ragnarok.3]
Name=Dragon
Enabled=True
PrefabName=Dragon
MaxSpawned=1
SpawnInterval=600
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

[Ragnarok.4]
Name=gd_king
Enabled=True
PrefabName=gd_king
MaxSpawned=1
SpawnInterval=600
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

[Ragnarok.5]
Name=Draugr Troopers
Enabled=True
PrefabName=Draugr
MaxSpawned=15
SpawnInterval=10
SpawnDistance=0
SpawnRadiusMin=0
SpawnRadiusMax=10
GroupSizeMin=5
GroupSizeMax=5
GroupSizeRadius=2
SpawnAtNight=True
SpawnAtDay=True
HuntPlayer=True
GroundOffset=5
MaxLevel=1
MinLevel=1

[Ragnarok.6]
Name=Skelly Troopers
Enabled=True
PrefabName=Skeleton
MaxSpawned=15
SpawnInterval=10
SpawnDistance=0
SpawnRadiusMin=0
SpawnRadiusMax=10
GroupSizeMin=5
GroupSizeMax=5
GroupSizeRadius=2
SpawnAtNight=True
SpawnAtDay=True
HuntPlayer=True
GroundOffset=5
MaxLevel=1
MinLevel=1

[Ragnarok.7]
Name=Fenring
Enabled=True
PrefabName=Fenring
MaxSpawned=1
SpawnInterval=600
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
