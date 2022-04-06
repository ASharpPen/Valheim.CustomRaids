using HarmonyLib;
using System;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Utilities.Extensions;

internal static class CodeMatcherExtensions
{
    internal static CodeMatcher Print(this CodeMatcher codeMatcher, int before, int after)
    {
#if DEBUG
        for (int i = -before; i <= after; ++i)
        {
            int currentOffset = i;
            int index = codeMatcher.Pos + currentOffset;

            if (index <= 0)
            {
                continue;
            }

            if (index >= codeMatcher.Length)
            {
                break;
            }

            try
            {
                var line = codeMatcher.InstructionAt(currentOffset);
                Log.LogTrace($"[{currentOffset}] " + line.ToString());
            }
            catch (Exception e)
            {
                Log.LogError(e.Message);
            }
        }
#endif
        return codeMatcher;
    }
}
