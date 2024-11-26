using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;

namespace VanillaQuestsExpandedTheGenerator
{
    public class JobDriver_StudyGenetron : JobDriver
    {

        public const int totalTime = 1200; // 20 seconds
        public int totalTimer = 0;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A).Thing, this.job, 1, -1, null, true);
        }
        private Building_Genetron_Studiable Building => (Building_Genetron_Studiable)job.GetTarget(TargetIndex.A).Thing;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Thing building = this.job.GetTarget(TargetIndex.A).Thing;
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);

            Toil study = ToilMaker.MakeToil("MakeNewToils");

            study.tickAction = delegate
            {
                Pawn actor = study.actor;
                if (actor.skills != null)
                {
                    actor.skills.Learn(SkillDefOf.Intellectual, 0.025f);
                }

                actor.rotationTracker.FaceTarget(actor.CurJob.GetTarget(TargetIndex.A));

                totalTimer++;
                if (totalTimer > totalTime)
                {
                    Building.alreadyStudied = true;
                    if (Building.cachedDetailsExtension != null)
                    {
                        if (Building.cachedDetailsExtension?.studyingHediff != null && !actor.health.hediffSet.HasHediff(Building.cachedDetailsExtension.studyingHediff))
                        {
                            actor.health.AddHediff(Building.cachedDetailsExtension.studyingHediff);
                        }
                        if (Building.cachedDetailsExtension.toggleGeothermalStudied)
                        {
                            Genetron_GameComponent.Instance.geothermalGenetronStudied = true;
                        }
                        if (Building.cachedDetailsExtension.toggleNuclearStudied)
                        {
                            Genetron_GameComponent.Instance.nuclearGenetronStudied = true;
                        }
                    }
                    

                    actor.jobs.EndCurrentJob(JobCondition.Succeeded);


                }
            };
       
            study.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            study.WithEffect(EffecterDefOf.Research, TargetIndex.A);
            study.defaultCompleteMode = ToilCompleteMode.Never;
            study.activeSkill = () => SkillDefOf.Intellectual;
            study.handlingFacing = true;
            yield return study;



        }
    }
}
