using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using VanillaQuestsExpandedTheGenerator;
using Verse;
using Verse.AI.Group;

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
                            var faction = Find.FactionManager.RandomEnemyFaction(allowNonHumanlike: false);
                            pawn.SetFaction(faction);

                            var map = pawn.Map;
                            LordMaker.MakeNewLord(faction, new LordJob_AssaultColony(faction, true, false, false, true, true, false, true), map, new List<Pawn> { pawn });
                            Find.LetterStack.ReceiveLetter("VQE_TraitorLabel".Translate(pawn.Named("PAWN")).AdjustedFor(pawn), "VQE_TraitorLetter".Translate(pawn.Named("PAWN")).AdjustedFor(pawn), LetterDefOf.ThreatBig, new TargetInfo(pawn.Position, map, false));

                            parent.pawn.health.hediffSet.hediffs.Remove(parent);

                        }

                        

                    }
                    SetOrResetTicks();
                }
                
            }
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