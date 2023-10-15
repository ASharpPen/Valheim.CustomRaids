using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Utilities.Extensions;

namespace Valheim.CustomRaids.TerminalCommands;

internal class ExplainPlayerReadyEventCommand
{
    public const string CommandName = "customraids:explain_player_event";

    internal static void Register()
    {
#if DEBUG
        Log.LogTrace($"Registering command '{CommandName}'");
#endif

        new Terminal.ConsoleCommand(
            CommandName,
            "Explain why event is ready or not for current player. This is only for the player-based part of event requirements.",
            (args) => Command(args.Context, args.Args),
            optionsFetcher: GetEventNames);
    }

    private static void Command(Terminal context, string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                context.AddString("Must specify an event name.");
                return;
            }

            if (Player.m_localPlayer.IsNull())
            {
                Log.LogTrace($"{CommandName}: local player is null. Skipping command.");
                return;
            }

            var randEvent = RandEventSystem.instance.m_events.FirstOrDefault(x => x.m_name == args[1]);

            if (randEvent is null)
            {
                context.AddString($"No such event '{args[1]}' is present.");
                return;
            }

            if (!randEvent.m_enabled)
            {
                context.AddString("Event is disabled.");
            }

            var explanation = Explain(randEvent);

            Log.LogTrace($"{CommandName}: {explanation.Join(delimiter: "\n")}");

            foreach(var entry in explanation)
            {
                context.AddString(entry);
            }
        }
        catch (Exception e)
        {
            Log.LogError($"Error while attempting to execute terminal command '{CommandName}'.", e);
        }
    }

    private static List<string> GetEventNames()
    {
        if (RandEventSystem.instance.IsNull())
        {
            return new List<string>();
        }

        return RandEventSystem.instance.m_events.Select(x => x.m_name).ToList();
    }

    private static List<string> Explain(RandomEvent randEvent)
    {
        List<string> results = new();

        Player player = Player.m_localPlayer;

        bool notKnownItems = true;
        bool notKeys = true;
        bool knowItems = false;
        bool keys = false;

        if (randEvent.m_altRequiredNotKnownItems?.Count > 0)
        {
            results.Add("Must not know items: ");
            foreach (ItemDrop itemDrop in randEvent.m_altRequiredNotKnownItems)
            {
                if (player.IsMaterialKnown(itemDrop.m_itemData.m_shared.m_name))
                {
                    results.Add($"  [ ] {itemDrop.name}");
                    notKnownItems = false;
                }
                else
                {
                    results.Add($"  [X] {itemDrop.name}");
                }
            }
        }

        if (randEvent.m_altNotRequiredPlayerKeys?.Count > 0)
        {
            results.Add("Must not have keys:");
            foreach (string name in randEvent.m_altNotRequiredPlayerKeys)
            {
                if (player.HaveUniqueKey(name))
                {
                    results.Add($"  [ ] {name}");
                    notKeys = false;
                }
                else
                {
                    results.Add($"  [X] {name}");
                }
            }
        }

        if (randEvent.m_altRequiredKnownItems?.Count > 0)
        {
            results.Add("Must know one of items: ");
            foreach (ItemDrop itemDrop in randEvent.m_altRequiredKnownItems)
            {
                if (player.IsMaterialKnown(itemDrop.m_itemData.m_shared.m_name))
                {
                    results.Add($"  [X] {itemDrop.name}");
                    knowItems = true;
                }
                else
                {
                    results.Add($"  [ ] {itemDrop.name}");
                }
            }
        }

        if (randEvent.m_altRequiredPlayerKeysAny?.Count > 0)
        {
            results.Add("Must have one of keys (ignored if a required known item is present):");
            foreach (string name in randEvent.m_altRequiredPlayerKeysAny)
            {
                if (player.HaveUniqueKey(name))
                {
                    results.Add($"  [X] {name}");
                    keys = true;
                }
                else
                {
                    results.Add($"  [ ] {name}");
                }
            }
        }

        if (!notKnownItems)
        {
            results.Add("Event disabled due to knowing item in \"must not know items\" list.");
        }
        else if (!notKeys)
        {
            results.Add("Event disabled due to having key in \"must not have keys\" list.");
        }
        else if (knowItems)
        {
            results.Add("Event enabled due to knowing item in \"must know one of items\" list.");
        }
        else if (keys)
        {
            results.Add("Event enabled due to having key in \"must know have one of keys\" list.");
        }
        else if (
            !knowItems && 
            !keys && 
            randEvent.m_altRequiredKnownItems.Count == 0 &&
            randEvent.m_altRequiredPlayerKeysAny.Count == 0)
        {
            results.Add("Event enabled due to having fulfilled all requirements.");
        }
        else
        {
            results.Add("Event disabled due to not having fulfilled all requirements.");
        }

        return results;
    }
}

