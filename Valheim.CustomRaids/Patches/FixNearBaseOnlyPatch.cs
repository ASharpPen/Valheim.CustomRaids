using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Valheim.CustomRaids.Patches
{
    [HarmonyPatch(typeof(RandEventSystem))]
    public static class FixNearBaseOnlyPatch
    {
        [HarmonyPatch("CheckBase")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> AddEscapeIfNotRelevant(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();

            var pos = -1;

            var labelRetTrue = new Label();

            for(int i = 0; i < codes.Count; ++i)
            {
                var code = codes[i];

                if(code.opcode == OpCodes.Ldc_I4_1)
                {
                    var nextCode = codes[i + 1];
                    if (nextCode.opcode == OpCodes.Ret)
                    {
                        codes[i] = code.WithLabels(labelRetTrue);
                        break;
                    }
                }
            }

            if(pos < 0)
            {
                Log.LogError("Unable to transpile in fix for NearBaseOnly=true requirement for raids.");
            }

            List<CodeInstruction> result = new List<CodeInstruction>();

            result.Add(new CodeInstruction(OpCodes.Ldarg_1));
            result.Add(CodeInstruction.LoadField(typeof(RandomEvent), "m_nearBaseOnly"));
            result.Add(new CodeInstruction(OpCodes.Brfalse_S, labelRetTrue));
            result.AddRange(instructions);

            return result;
        }
    }
}
