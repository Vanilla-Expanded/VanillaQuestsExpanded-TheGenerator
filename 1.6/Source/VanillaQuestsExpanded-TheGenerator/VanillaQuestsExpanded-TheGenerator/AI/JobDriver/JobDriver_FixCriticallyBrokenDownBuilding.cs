using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;
namespace VanillaQuestsExpandedTheGenerator
{
    public class JobDriver_FixCriticallyBrokenDownBuilding : JobDriver
    {
        private const TargetIndex BuildingInd = TargetIndex.A;

        private const TargetIndex ComponentInd = TargetIndex.B;

        private const int TicksDuration = 1000;

        private Building_GenetronOverdrive Building => (Building_GenetronOverdrive)job.GetTarget(TargetIndex.A).Thing;

        private Thing Components => job.GetTarget(TargetIndex.B).Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.Reserve(Building, job, 1, -1, null, errorOnFailed))
            {
                return pawn.Reserve(Components, job, 1, -1, null, errorOnFailed);
            }
            return false;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            Toil toil = Toils_General.Wait(1000);
            toil.FailOnDespawnedOrNull(TargetIndex.A);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            toil.WithEffect(Building.def.repairEffect, TargetIndex.A);
            toil.WithProgressBarToilDelay(TargetIndex.A);
            toil.activeSkill = (() => SkillDefOf.Construction);
            yield return toil;
            Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
            toil2.initAction = delegate
            {
                Components.Destroy();
                if (Rand.Value > pawn.GetStatValue(StatDefOf.FixBrokenDownBuildingSuccessChance))
                {
                    MoteMaker.ThrowText((pawn.DrawPos + Building.DrawPos) / 2f, base.Map, "TextMote_FixBrokenDownBuildingFail".Translate(), 3.65f);
                }
                else
                {
                    Building.Signal_CriticalBreakdownRepaired();
                }
            };
            yield return toil2;
        }
    }
}