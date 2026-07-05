using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
   
    [HarmonyPatch(typeof(ResearchProjectDef))]
    [HarmonyPatch("UnlockedDefs", MethodType.Getter)]
    public static class VanillaQuestsExpandedTheGenerator_ResearchProjectDef_UnlockedDefs_Patch
    {
        public static void Postfix(ref ResearchProjectDef __instance, ref List<Def> __result)
        {
            if (__result.Contains(InternalDefOf.VQE_Genetron_Basic)) {
                List<Def> newList = __result;
                newList.Remove(InternalDefOf.VQE_Genetron_Basic);
                __result = newList;
            }

        }
    }
}
