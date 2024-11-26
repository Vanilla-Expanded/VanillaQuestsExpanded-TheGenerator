
using HarmonyLib;
using RimWorld;
using Verse;
using System;
using Verse.AI;



namespace VanillaQuestsExpandedTheGenerator
{
    [HarmonyPatch(typeof(GenConstruct), "CanConstruct", new Type[] { typeof(Thing), typeof(Pawn), typeof(bool), typeof(bool), typeof(JobDef) })]
    internal class VanillaQuestsExpandedTheGenerator_GenConstruct_CanConstruct_Postfix
    {
        [HarmonyPostfix]
        private static void PostFix(ref bool __result, Thing t, Pawn p)
        {

            if (__result && StaticCollections.genetrons.Contains(t.def.entityDefToBuild) && !StaticCollections.geothermalGenetrons.Contains(t.def.entityDefToBuild))
            {
                if(t.def.entityDefToBuild==InternalDefOf.VQE_Genetron_Geothermal && !Utils.HasStudiedGeothermalHediffOrBackstory(p))
                {
                    if (p != null) { JobFailReason.Is("VQE_NeedsStudyGeothermal".Translate(p.NameFullColored)); }

                    __result = false;
                }
                else
                if (StaticCollections.nuclearGenetrons.Contains(t.def.entityDefToBuild) && !Utils.HasStudiedNuclearHediffOrBackstory(p))
                {
                    if (p != null) { JobFailReason.Is("VQE_NeedsStudyNuclear".Translate(p.NameFullColored)); }

                    __result = false;
                }
                else


                if (!Utils.HasStudiedBasicHediffOrBackstory(p))
                {
                    if(p!=null) { JobFailReason.Is("VQE_NeedsStudy".Translate(p.NameFullColored)); }
                    
                    __result = false;
                }

            }

        }
    }
}