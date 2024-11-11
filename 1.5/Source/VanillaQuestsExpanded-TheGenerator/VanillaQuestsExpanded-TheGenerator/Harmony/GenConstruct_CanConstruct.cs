
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

            if (__result && StaticCollections.genetrons.Contains(t.def.entityDefToBuild))
            {

                if (p?.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VQE_StudiedAncientGenetron)==null)
                {
                    if(p!=null) { JobFailReason.Is("VQE_NeedsStudy".Translate(p.NameFullColored)); }
                    
                    __result = false;
                }

            }

        }
    }
}