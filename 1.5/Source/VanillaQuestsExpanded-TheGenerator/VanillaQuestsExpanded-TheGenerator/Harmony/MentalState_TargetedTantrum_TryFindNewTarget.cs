using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using Verse.AI;
using static UnityEngine.GraphicsBuffer;



namespace VanillaQuestsExpandedTheGenerator
{


    [HarmonyPatch(typeof(MentalState_TargetedTantrum))]
    [HarmonyPatch("TryFindNewTarget")]
    public static class VanillaQuestsExpandedTheGenerator_MentalState_TargetedTantrum_TryFindNewTarget_Patch
    {

        private static List<Thing> tmpThings = new List<Thing>();

        [HarmonyPostfix]
        static void TargetARC(MentalState_TargetedTantrum __instance, ref bool __result)
        {
            if (__instance.pawn.Ideo?.HasPrecept(InternalDefOf.VQE_ARCGenerators_Abhorrent) == true)
            {
                GetSmashableARCsNear(__instance.pawn, __instance.pawn.Position, tmpThings, null, 300);
                bool result = tmpThings.TryRandomElementByWeight((Thing x) => x.MarketValue * (float)x.stackCount, out __instance.target);
                tmpThings.Clear();
                __result= result;

            }else if (__instance.pawn.Ideo?.HasPrecept(InternalDefOf.VQE_Technology_Rejected) == true)
            {
                TantrumMentalStateUtility.GetSmashableThingsNear(__instance.pawn, __instance.pawn.Position, tmpThings, GetCustomValidator());
                bool result = tmpThings.TryRandomElementByWeight((Thing x) => x.MarketValue * (float)x.stackCount, out __instance.target);
                tmpThings.Clear();
                __result = result;

            }

        }

        public static Predicate<Thing> GetCustomValidator()
        {
            return (Thing x) => x.def.techLevel > TechLevel.Medieval;
        }

        public static void GetSmashableARCsNear(Pawn pawn, IntVec3 near, List<Thing> outCandidates, Predicate<Thing> customValidator = null, int extraMinBuildingOrItemMarketValue = 0, int maxDistance = 40)
        {
            outCandidates.Clear();
            if (!pawn.CanReach(near, PathEndMode.OnCell, Danger.Deadly))
            {
                return;
            }
            Region region = near.GetRegion(pawn.Map);
            if (region == null)
            {
                return;
            }
            TraverseParms traverseParams = TraverseParms.For(pawn);
            RegionTraverser.BreadthFirstTraverse(region, (Region from, Region to) => to.Allows(traverseParams, isDestination: false), delegate (Region r)
            {
                List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Building_Genetron && list[i].Position.InHorDistOf(near, maxDistance) && TantrumMentalStateUtility.CanSmash(pawn, list[i], skipReachabilityCheck: true, customValidator, extraMinBuildingOrItemMarketValue))
                    {
                        outCandidates.Add(list[i]);
                    }
                }
              
                return false;
            }, 40);
        }

    }








}
