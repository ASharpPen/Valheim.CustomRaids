using HarmonyLib;
using System;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Core.Network;

namespace Valheim.CustomRaids.Configuration.Multiplayer
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

				DataTransferService.Service.AddToQueue(new ConfigPackage().Pack(), nameof(RPC_ReceiveConfigsCustomRaids), rpc);

				Log.LogTrace("Sending config packages.");
			}
			catch (Exception e)
			{
				Log.LogError("Unexpected error while attempting to create and send config package from server to client.", e);
			}
		}

		private static void RPC_ReceiveConfigsCustomRaids(ZRpc rpc, ZPackage pkg)
		{
			Log.LogInfo("Received package.");
			try
			{
				CompressedPackage.Unpack(pkg);
			}
			catch (Exception e)
			{
				Log.LogError("Error while attempting to read received config package.", e);
			}
		}
	}
}
