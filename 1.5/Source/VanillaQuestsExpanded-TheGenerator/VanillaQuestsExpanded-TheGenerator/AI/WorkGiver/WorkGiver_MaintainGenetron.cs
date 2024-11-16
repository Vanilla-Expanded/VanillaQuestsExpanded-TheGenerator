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

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_GenetronWithMaintenance building = t as Building_GenetronWithMaintenance;
            bool result;
            if (t == null || t.IsBurning() || building.maintenance > 0.7f)
            {

                result = false;
            }
            else
            {
                if (pawn?.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VQE_StudiedAncientGenetron) == null)
                {
                    JobFailReason.Is(NotStudied);
                    return false;
                }
                if (!t.IsForbidden(pawn))
                {

                    if (pawn.CanReserve(t, 1, -1, null, forced))
                    {

                        return true;
                    }
                }
                result = false;
            }
            return result;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(InternalDefOf.VQE_MaintainGenetron, t);
        }
    }
}
