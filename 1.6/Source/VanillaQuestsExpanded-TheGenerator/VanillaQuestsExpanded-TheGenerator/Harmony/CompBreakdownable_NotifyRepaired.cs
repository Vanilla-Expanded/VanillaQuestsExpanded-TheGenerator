using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{

    [HarmonyPatch(typeof(CompBreakdownable))]
    [HarmonyPatch("Notify_Repaired")]
    public static class VanillaQuestsExpandedTheGenerator_CompBreakdownable_Notify_Repaired_Patch
    {

        [HarmonyPostfix]
        static void NotifyComponentCalibration(CompBreakdownable __instance)
        {
            if(__instance.parent is Building_GenetronWithComponentCalibration genetron && genetron.inBreakdown)
            {
                genetron.Signal_Repaired();
            }

        }
    }








}
