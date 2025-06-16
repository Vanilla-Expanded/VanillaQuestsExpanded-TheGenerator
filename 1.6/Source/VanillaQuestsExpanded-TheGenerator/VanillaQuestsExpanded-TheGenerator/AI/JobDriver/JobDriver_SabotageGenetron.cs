using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;

namespace VanillaQuestsExpandedTheGenerator
{
    public class JobDriver_SabotageGenetron : JobDriver
    {


        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A).Thing, this.job, 1, -1, null, true);
        }
        private Building_Genetron Building => (Building_Genetron)job.GetTarget(TargetIndex.A).Thing;

        protected override IEnumerable<Toil> MakeNewToils()
        {
           
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);          
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);

            Toil toil = Toils_General.Wait(300);
          
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            toil.WithEffect(Building.def.repairEffect, TargetIndex.A);
            toil.WithProgressBarToilDelay(TargetIndex.A);
           
            yield return toil;
            Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
            toil2.initAction = delegate
            {


                if(Building is Building_GenetronOverdrive building_Overdrive)
                {
                    building_Overdrive.Signal_ShortFuse();
                }
                else { 
                    Building.Signal_Explode();
                    Building.Destroy();
                }
                
            };
            yield return toil2;



        }
    }
}
