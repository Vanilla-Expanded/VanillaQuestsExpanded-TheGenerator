using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;




namespace VanillaQuestsExpandedTheGenerator
{


    [HarmonyPatch(typeof(GenSpawn))]
    [HarmonyPatch("SpawningWipes")]
    public static class VanillaQuestsExpandedTheGenerator_GenSpawn_SpawningWipes_Patch
    {
        public static HashSet<ThingDef> genetrons = new HashSet<ThingDef>() { InternalDefOf.VQE_Genetron_Basic,InternalDefOf.VQE_Genetron_WoodFired};


        [HarmonyPostfix]
        static void DisableDeconstruct(BuildableDef newEntDef, BuildableDef oldEntDef, ref bool __result)
        {
            ThingDef thingDef = newEntDef as ThingDef;
            ThingDef thingDef2 = oldEntDef as ThingDef;
           
            if ((genetrons.Contains(thingDef)||thingDef.defName.Contains("Frame_")) && genetrons.Contains(thingDef2))
            {
                __result = false;

            }



        }
    }








}
