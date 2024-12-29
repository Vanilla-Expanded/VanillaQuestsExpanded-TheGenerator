using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using VFECore;

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
			if (TargetA.Thing.def.hasInteractionCell)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			}

			Toil study = ToilMaker.MakeToil("MakeNewToils");
			study.initAction = delegate
			{
				if (!Building.studyStartedSignal.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(Building.studyStartedSignal, Building.Named("SUBJECT")));
				}
				QuestUtility.SendQuestTargetSignals(Building.questTags, "StudyStarted", Building.Named("SUBJECT"));
			};
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
					if (!Building.studyFinishedSignal.NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(Building.studyFinishedSignal, Building.Named("SUBJECT")));
					}
					QuestUtility.SendQuestTargetSignals(Building.questTags, "Studied", Building.Named("SUBJECT"));
					Building.studied = true;
					var ext = Building.cachedDetailsExtension;
					if (ext != null)
					{
						Messages.Message("VQE_HasStudied".Translate(actor.NameShortColored,Building.LabelCap), MessageTypeDefOf.PositiveEvent, true);


						if (ext.studyingHediff != null && !actor.health.hediffSet.HasHediff(ext.studyingHediff))
						{
							actor.health.AddHediff(ext.studyingHediff);
							Genetron_GameComponent.Instance.anyGenetronStudied = true;
						}
						if (ext.toggleGeothermalStudied)
						{
							Genetron_GameComponent.Instance.geothermalGenetronStudied = true;
						}
						if (ext.toggleNuclearStudied)
						{
							Genetron_GameComponent.Instance.nuclearGenetronStudied = true;
						}
						Building.GetComp<CompBouncingArrow>().doBouncingArrow = false;
						if (ext.convertAfterStudying != null)
						{
							var pos = Building.Position;
							var rot = Building.Rotation;
							var map = Building.Map;
							Building.preventSendingSignal = true;
							Building.Destroy();
							var newThing = ThingMaker.MakeThing(ext.convertAfterStudying);
							GenSpawn.Spawn(newThing, pos, map, rot);
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
