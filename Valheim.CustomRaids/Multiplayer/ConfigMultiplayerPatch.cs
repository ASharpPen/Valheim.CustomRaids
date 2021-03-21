using HarmonyLib;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Valheim.CustomRaids.ConfigurationTypes;

namespace Valheim.CustomRaids.Multiplayer
{
	[HarmonyPatch(typeof(ZNet))]
	public class ConfigMultiplayerPatch
	{
		[HarmonyPatch("OnNewConnection")]
		[HarmonyPostfix]
		private static void SyncConfigs(ZNet __instance, ZNetPeer peer)
		{
			if (ZNet.instance.IsServer())
			{
				Log.LogDebug("Registering server RPC for sending configs on request from client.");
				peer.m_rpc.Register(nameof(RPC_RequestConfigsCustomRaids), new ZRpc.RpcMethod.Method(RPC_RequestConfigsCustomRaids));
			}
			else
			{
				Log.LogDebug("Registering client RPC for receiving configs from server.");
				peer.m_rpc.Register<ZPackage>(nameof(RPC_ReceiveConfigsCustomRaids), new Action<ZRpc, ZPackage>(RPC_ReceiveConfigsCustomRaids));

				Log.LogDebug("Requesting configs from server.");
				peer.m_rpc.Invoke(nameof(RPC_RequestConfigsCustomRaids));
			}
		}

		private static void RPC_RequestConfigsCustomRaids(ZRpc rpc)
		{
			try
			{
				if (!ZNet.instance.IsServer())
				{
					Log.LogWarning("Non-server instance received request for configs. Ignoring request.");
				}

				Log.LogInfo("Received request for configs.");

				ZPackage configPackage = new ZPackage();

				var package = new ConfigurationPackage(
					ConfigurationManager.GeneralConfig, 
					ConfigurationManager.RaidConfig);

				Log.LogTrace("Serializing configs.");

				using (MemoryStream memStream = new MemoryStream())
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					binaryFormatter.Serialize(memStream, package);

					byte[] serialized = memStream.ToArray();

					configPackage.Write(serialized);
				}

				Log.LogTrace("Sending config package.");

				rpc.Invoke(nameof(RPC_ReceiveConfigsCustomRaids), new object[] { configPackage });

				Log.LogTrace("Finished sending config package.");
			}
			catch (Exception e)
            {
				Log.LogError("Unexpected error while attempting to create and send config package from server to client.", e);
            }
		}

		private static void RPC_ReceiveConfigsCustomRaids(ZRpc rpc, ZPackage pkg)
		{
			Log.LogTrace("Received package.");
			try
			{
				var serialized = pkg.ReadByteArray();

				Log.LogTrace("Deserializing package.");

				using (MemoryStream memStream = new MemoryStream(serialized))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					var responseObject = binaryFormatter.Deserialize(memStream);

					if (responseObject is ConfigurationPackage configPackage)
					{
						Log.LogDebug("Received and deserialized config package");

						Log.LogTrace("Unpackaging general config.");

						ConfigurationManager.GeneralConfig = configPackage.GeneralConfig;

						Log.LogTrace("Successfully set general config.");
						Log.LogTrace("Unpackaging raid event configs.");

						ConfigurationManager.RaidConfig = configPackage.RaidConfigs;

						Log.LogTrace("Successfully set raid event configs.");

						RandEventSystemPatch.ApplyConfigurations();
					}
					else
					{
						Log.LogWarning("Received bad config package. Unable to load.");
					}
				}
			}
			catch(Exception e)
            {
				Log.LogError("Error while attempting to read received config package.", e);
            }
		}
	}
}
