using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using VanillaQuestsExpandedTheGenerator;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Noise;

namespace VanillaQuestsExpandedTheGenerator
{
    public class HediffComp_Traitor : HediffComp
    {
        public HediffCompPropreties_Traitor Props => (HediffCompPropreties_Traitor)props;

        private int ticksToDisappear;

        public override void CompPostMake()
        {
            SetOrResetTicks();
        }

        public void SetOrResetTicks()
        {
            ticksToDisappear = Props.disappearsAfterTicks.RandomInRange;
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            var pawn = Pawn;
            if (pawn.Spawned && pawn.IsHashIntervalTick(250))
            {
                ticksToDisappear -= 250;
                if (ticksToDisappear <= 0)
                {
                    if(pawn.Map!=null) {
                        List<Building_Genetron> listOfThings = pawn.Map.listerBuildings.AllColonistBuildingsOfType<Building_Genetron>().ToList();
                        if(listOfThings.Count > 0)
                        {
                            Building_Genetron genetron = listOfThings.First();

                            if(CheckJobPossible(pawn, genetron))
                            {

                                var faction = Find.FactionManager.RandomEnemyFaction(allowNonHumanlike: false);
                                pawn.SetFaction(faction);
                                ReturnJob(pawn, genetron);
                                Find.LetterStack.ReceiveLetter("VQE_TraitorLabel".Translate(pawn.Named("PAWN")).AdjustedFor(pawn), "VQE_TraitorLetter".Translate(pawn.Named("PAWN")).AdjustedFor(pawn), LetterDefOf.ThreatBig, new TargetInfo(pawn.Position, pawn.Map, false));
                                parent.pawn.health.hediffSet.hediffs.Remove(parent);

                            }
                       
                        }                  
                    }
                    SetOrResetTicks();
                }
                
            }
        }

        public bool CheckJobPossible(Pawn pawn, Building_Genetron building)
        {
            
            if (building == null)
            {
                return false;
            }           

            if (building.IsForbidden(pawn))
            {
                return false;
            }

            if (!pawn.CanReserve(building, 1, -1, null, true))
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

        public void ReturnJob(Pawn pawn, Thing t, bool forced = false)
        {
            pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.VQE_SabotageGenetron, t), JobTag.Misc);
             
        }

        public override void CompPostMerged(Hediff other)
        {
            var hediffComp = other.TryGetComp<HediffComp_Traitor>();
            if (hediffComp != null && hediffComp.ticksToDisappear > ticksToDisappear)
                ticksToDisappear = hediffComp.ticksToDisappear;
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref ticksToDisappear, "ticksToDisappear");
        }

        public override string CompDebugString()
        {
            return "ticksToDisappear: " + ticksToDisappear;
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            if (Prefs.DevMode)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEBUG: Traitor appears",
                    action = new Action(() =>
                    {
                        ticksToDisappear = 200;
                    })
                };
            }
        }
    }
}