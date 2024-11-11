
using RimWorld;
using System.Collections.Generic;
using System;
using Verse;
using Verse.AI;
namespace VanillaQuestsExpandedTheGenerator
{
    public class WorkGiver_RefuelWithOverdrive : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(InternalDefOf.VQE_Genetron_WoodPowered);


        public override PathEndMode PathEndMode => PathEndMode.Touch;

        public virtual JobDef JobStandard => JobDefOf.Refuel;

        public virtual JobDef JobAtomic => JobDefOf.RefuelAtomic;

        public virtual bool CanRefuelThing(Thing t)
        {
            return !(t is Building_Turret);
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (CanRefuelThing(t))
            {
                return CanRefuel(pawn, t, forced);
            }
            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return RefuelWorkGiverUtility.RefuelJob(pawn, t, forced, JobStandard, JobAtomic);
        }

        public static bool CanRefuel(Pawn pawn, Thing t, bool forced = false)
        {
            CompRefuelableWithOverdrive compRefuelable = t.TryGetComp<CompRefuelableWithOverdrive>();
            if (compRefuelable == null || compRefuelable.IsFull || (!forced && !compRefuelable.allowAutoRefuel))
            {
                return false;
            }
            if (compRefuelable.FuelPercentOfMax > 0f && !compRefuelable.Props.allowRefuelIfNotEmpty)
            {
                return false;
            }
            if (!forced && !compRefuelable.ShouldAutoRefuelNow)
            {
                return false;
            }
            if (t.IsForbidden(pawn) || !pawn.CanReserve(t, 1, -1, null, forced))
            {
                return false;
            }
            if (t.Faction != pawn.Faction)
            {
                return false;
            }
            CompInteractable compInteractable = t.TryGetComp<CompInteractable>();
            if (compInteractable != null && compInteractable.Props.cooldownPreventsRefuel && compInteractable.OnCooldown)
            {
                JobFailReason.Is(compInteractable.Props.onCooldownString.CapitalizeFirst());
                return false;
            }
            if (FindBestFuel(pawn, t) == null)
            {
                ThingFilter fuelFilter = t.TryGetComp<CompRefuelableWithOverdrive>().Props.fuelFilter;
                JobFailReason.Is("NoFuelToRefuel".Translate(fuelFilter.Summary));
                return false;
            }
            if (t.TryGetComp<CompRefuelableWithOverdrive>().Props.atomicFueling && FindAllFuel(pawn, t) == null)
            {
                ThingFilter fuelFilter2 = t.TryGetComp<CompRefuelableWithOverdrive>().Props.fuelFilter;
                JobFailReason.Is("NoFuelToRefuel".Translate(fuelFilter2.Summary));
                return false;
            }
            return true;
        }
        private static Thing FindBestFuel(Pawn pawn, Thing refuelable)
        {
            ThingFilter filter = refuelable.TryGetComp<CompRefuelableWithOverdrive>().Props.fuelFilter;
            Predicate<Thing> validator = delegate (Thing x)
            {
                if (x.IsForbidden(pawn) || !pawn.CanReserve(x))
                {
                    return false;
                }
                return filter.Allows(x) ? true : false;
            };
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, filter.BestThingRequest, PathEndMode.ClosestTouch, TraverseParms.For(pawn), 9999f, validator);
        }

        private static List<Thing> FindAllFuel(Pawn pawn, Thing refuelable)
        {
            int fuelCountToFullyRefuel = refuelable.TryGetComp<CompRefuelableWithOverdrive>().GetFuelCountToFullyRefuel();
            ThingFilter filter = refuelable.TryGetComp<CompRefuelableWithOverdrive>().Props.fuelFilter;
            return RefuelWorkGiverUtility.FindEnoughReservableThings(pawn, refuelable.Position, new IntRange(fuelCountToFullyRefuel, fuelCountToFullyRefuel), (Thing t) => filter.Allows(t));
        }

    }
}
