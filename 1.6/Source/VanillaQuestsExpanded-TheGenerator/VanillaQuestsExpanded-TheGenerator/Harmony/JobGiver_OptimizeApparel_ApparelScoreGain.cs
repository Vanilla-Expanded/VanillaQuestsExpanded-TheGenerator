
using HarmonyLib;
using RimWorld;
using Verse;
using System;



namespace VanillaQuestsExpandedTheGenerator
{
    [HarmonyPatch(typeof(JobGiver_OptimizeApparel), "ApparelScoreGain")]
    internal class VanillaQuestsExpandedTheGenerator_JobGiver_OptimizeApparel_ApparelScoreGain_Postfix
    {


        [HarmonyPostfix]
        private static void PostFix(ref float __result, Pawn pawn, Apparel ap)
        {
            if (pawn.Ideo?.HasPrecept(InternalDefOf.VQE_Technology_Rejected) == true && ap.def.techLevel > TechLevel.Medieval)
            {
                __result = -1000;

            }


        }
    }
}