using System;
using System.Collections.Generic;
using RimWorld;
using VanillaQuestsExpandedTheGenerator;
using Verse;
using Verse.AI;

namespace VanillaQuestsExpandedTheGenerator
{
    public class WorkGiver_StudyGenetron : WorkGiver_Scanner
    {
       

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.GetComponent<Genetron_MapComponent>().studiables_InMap;
        }



        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

      

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            return pawn.Map.GetComponent<Genetron_MapComponent>().studiables_InMap.Count == 0;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_Genetron_Studiable building = t as Building_Genetron_Studiable;

            if (building == null)
            {
                return false;
            }

            if (building.alreadyStudied)
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
           
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(InternalDefOf.VQE_StudyGenetron, t);
        }
    }
}
