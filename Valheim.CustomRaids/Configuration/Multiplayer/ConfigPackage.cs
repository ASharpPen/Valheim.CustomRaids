using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids2.Configs.BepInEx;

namespace Valheim.CustomRaids.Configuration.Multiplayer
{
    [Serializable]
    internal class ConfigPackage
    {
        public GeneralConfiguration GeneralConfig;

        public RaidEventConfigurationFile RaidConfig;

        public ZPackage Pack()
        {
            ZPackage package = new ZPackage();

            GeneralConfig = ConfigurationManager.GeneralConfig;
            RaidConfig = ConfigurationManager.RaidConfig;

            Log.LogTrace("Serializing configs.");

            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memStream, this);

                byte[] serialized = memStream.ToArray();

                package.Write(serialized);
            }

            return package;
        }

        public static void Unpack(ZPackage package)
        {
            var serialized = package.ReadByteArray();

            Log.LogTrace("Deserializing package.");
            Log.LogTrace("Package content size in bytes: " + package.Size());

            using (MemoryStream memStream = new MemoryStream(serialized))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                var responseObject = binaryFormatter.Deserialize(memStream);

                if (responseObject is ConfigPackage configPackage)
                {
                    Log.LogDebug("Received and deserialized config package");

                    Log.LogTrace("Unpackaging configs.");

                    ConfigurationManager.GeneralConfig = configPackage.GeneralConfig;
                    ConfigurationManager.RaidConfig = configPackage.RaidConfig;

                    Log.LogTrace("Successfully unpacked configs.");

                    Log.LogTrace($"Unpacked general configs");
                    Log.LogTrace($"Unpacked {ConfigurationManager.RaidConfig?.Subsections?.Count ?? 0} raids");
                }
                else
                {
                    Log.LogWarning("Received bad config package. Unable to load.");
                }
            }
        }
    }
}
