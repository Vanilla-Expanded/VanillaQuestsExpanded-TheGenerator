using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{

    [HarmonyPatch(typeof(BuildCopyCommandUtility))]
    [HarmonyPatch("BuildCopyCommand")]
    public static class VanillaQuestsExpandedTheGenerator_BuildCopyCommandUtility_BuildCopyCommand_Patch
    {

        [HarmonyPostfix]
        static void RemoveCopyButton(BuildableDef buildable, ref Command __result)
        {
            if (StaticCollections.genetrons.Contains(buildable))
            {
                __result = null;
            }

        }
    }








}
