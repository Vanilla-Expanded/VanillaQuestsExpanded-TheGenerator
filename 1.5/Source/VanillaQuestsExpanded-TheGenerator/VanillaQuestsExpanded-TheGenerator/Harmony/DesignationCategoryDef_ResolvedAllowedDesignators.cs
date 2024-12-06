using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using RimWorld.BaseGen;



namespace VanillaQuestsExpandedTheGenerator
{


    [HarmonyPatch(typeof(DesignationCategoryDef))]
    [HarmonyPatch("ResolvedAllowedDesignators", MethodType.Getter)]


    public static class VanillaQuestsExpandedTheGenerator_DesignationCategoryDef_ResolvedAllowedDesignators_Patch
    {


        [HarmonyPostfix]
        public static IEnumerable<Designator> AllowBuild(IEnumerable<Designator> values)

        {
            IEnumerable<Designator> designators = values;

            foreach (Designator designator in designators)
            {
                Designator_Build designator_build = designator as Designator_Build;

                if (designator_build == null || !StaticCollections.hidden_designators.Contains(designator_build.PlacingDef))
                {
                    if (designator_build?.PlacingDef != InternalDefOf.VQE_Genetron_Basic || (designator_build.PlacingDef == InternalDefOf.VQE_Genetron_Basic && Genetron_GameComponent.Instance.anyGenetronStudied))
                    {

                        yield return designator;
                    }
                }


            }




        }

    }


}