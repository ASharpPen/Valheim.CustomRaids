using System;
using System.Linq;
using HarmonyLib;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Utilities.Extensions;

namespace Valheim.CustomRaids.TerminalCommands;

internal class ListPlayerReadyEventsCommand
{
    public const string CommandName = "customraids:list_player_events";

    internal static void Register()
    {
#if DEBUG
        Log.LogTrace($"Registering command '{CommandName}'");
#endif

        new Terminal.ConsoleCommand(
            CommandName,
            "List events ready for current player",
            (args) => Command(args.Context));
    }

    private static void Command(Terminal context)
    {
        try
        {
            if (Player.m_localPlayer.IsNull())
            {
                Log.LogTrace($"{CommandName}: local player is null. Skipping command.");
                return;
            }

            var keys = "  " + Player.m_localPlayer.m_readyEvents?.Join(delimiter: "\n  ") ?? "";

            Log.LogTrace($"{CommandName}: \n{keys}");

            context.AddString(keys);
        }
        catch (Exception e)
        {
            Log.LogError($"Error while attempting to execute terminal command '{CommandName}'.", e);
        }
    }
}
