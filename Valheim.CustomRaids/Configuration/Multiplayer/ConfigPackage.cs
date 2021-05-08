using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Configuration.Multiplayer
{
    [Serializable]
    internal class ConfigPackage
    {
        public GeneralConfiguration GeneralConfig;

        public ZPackage Pack()
        {
            ZPackage package = new ZPackage();

            GeneralConfig = ConfigurationManager.GeneralConfig;

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

            using (MemoryStream memStream = new MemoryStream(serialized))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                var responseObject = binaryFormatter.Deserialize(memStream);

                if (responseObject is ConfigPackage configPackage)
                {
                    Log.LogDebug("Received and deserialized config package");

                    Log.LogTrace("Unpackaging configs.");

                    ConfigurationManager.GeneralConfig = configPackage.GeneralConfig;

                    Log.LogTrace("Successfully unpacked configs.");

                    Log.LogTrace($"Unpacked general config");
                    //Log.LogTrace($"Unpacked {ConfigurationManager.CreatureSpawnerConfig?.Subsections?.Count ?? 0} creature spawner entries");
                }
                else
                {
                    Log.LogWarning("Received bad config package. Unable to load.");
                }
            }
        }
    }
}
