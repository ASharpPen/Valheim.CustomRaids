using System;
using System.Linq;
using HarmonyLib;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Utilities.Extensions;

namespace Valheim.CustomRaids.TerminalCommands;

internal class ListPlayerKeysCommand
{
    public const string CommandName = "customraids:list_player_keys";

    internal static void Register()
    {
#if DEBUG
        Log.LogTrace($"Registering command '{CommandName}'");
#endif

        new Terminal.ConsoleCommand(
            CommandName,
            "List player-specific key entries",
            (args) => ListPlayerKeys(args.Context));
    }

    private static void ListPlayerKeys(Terminal context)
    {
        try
        {
            if (Player.m_localPlayer.IsNull())
            {
                Log.LogTrace($"{CommandName}: local player is null. Skipping command.");
                return;
            }

            var keys = Player.m_localPlayer.GetUniqueKeys().Join();

            Log.LogTrace($"{CommandName}: {keys}");

            context.AddString(keys);
        }
        catch (Exception e)
        {
            Log.LogError($"Error while attempting to execute terminal command '{CommandName}'.", e);
        }
    }
}
