using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Utilities.Extensions
{
    internal static class CodeMatcherExtensions
    {
        public static CodeMatcher GetPosition(this CodeMatcher codeMatcher, out int position)
        {
            position = codeMatcher.Pos;
            return codeMatcher;
        }

        public static CodeMatcher AddLabel(this CodeMatcher codeMatcher, out Label label)
        {
            label = new Label();
            codeMatcher.AddLabels(new[] { label });
            return codeMatcher;
        }
    }
}
