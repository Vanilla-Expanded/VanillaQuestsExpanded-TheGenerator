using System;
using System.Collections.Generic;
using RimWorld;
using VanillaQuestsExpandedTheGenerator;
using Verse;
using Verse.AI;

namespace VanillaQuestsExpandedTheGenerator
{
    public class WorkGiver_MaintainGenetron : WorkGiver_Scanner
    {
        public static string NotStudied;

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.GetComponent<Genetron_MapComponent>().maintainables_InMap;
        }

        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);


        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public static void ResetStaticData()
        {
           
            NotStudied = "VQE_NeedsStudyGeneric".Translate();
        }

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            return pawn.Map.GetComponent<Genetron_MapComponent>().maintainables_InMap.Count == 0;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_GenetronWithMaintenance building = t as Building_GenetronWithMaintenance;

            if (building == null)
            {
                return false;
            }
           
            if (t.Faction != pawn.Faction)
            {
                return false;
            }
          
            if (t.IsForbidden(pawn))
            {
                return false;
            }
            if (building.cachedDetailsExtension?.anyoneCanHandle != true && pawn?.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VQE_StudiedAncientGenetron) == null)
            {
                JobFailReason.Is(NotStudied);
                return false;
            }
            if (!pawn.CanReserve(building, 1, -1, null, forced))
            {
                return false;
            }
            if (pawn.Map.designationManager.DesignationOn(building, DesignationDefOf.Deconstruct) != null)
            {
                return false;
            }
            if (building.IsBurning())
            {
                return false;
            }

            if (building.maintenance > 0.7f)
            {

                return false;
            }
           
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(InternalDefOf.VQE_MaintainGenetron, t);
        }
    }
}
