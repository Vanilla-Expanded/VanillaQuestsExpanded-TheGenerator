
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
                if(t.def.entityDefToBuild==InternalDefOf.VQE_Genetron_Geothermal && p?.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VQE_StudiedAncientGeothermalGenetron) == null)
                {
                    if (p != null) { JobFailReason.Is("VQE_NeedsStudyGeothermal".Translate(p.NameFullColored)); }

                    __result = false;
                }
                else
                if (StaticCollections.nuclearGenetrons.Contains(t.def.entityDefToBuild) && p?.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VQE_StudiedAncientNuclearGenetron) == null)
                {
                    if (p != null) { JobFailReason.Is("VQE_NeedsStudyNuclear".Translate(p.NameFullColored)); }

                    __result = false;
                }
                else


                if (p?.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VQE_StudiedAncientGenetron)==null)
                {
                    if(p!=null) { JobFailReason.Is("VQE_NeedsStudy".Translate(p.NameFullColored)); }
                    
                    __result = false;
                }

            }

        }
    }
}