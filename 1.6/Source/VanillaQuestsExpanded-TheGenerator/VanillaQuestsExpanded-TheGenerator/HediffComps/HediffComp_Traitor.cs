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

        public HashSet<MemeDef> memesToRemove = new HashSet<MemeDef>() { InternalDefOf.Transhumanist,InternalDefOf.VME_HardcoreIndustrialism,
        InternalDefOf.VME_MechanoidSupremacy, InternalDefOf.VME_Progressive};

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
                    if (pawn.Map != null)
                    {

                        if (Find.IdeoManager.classicMode)
                        {
                            List<Building_Genetron> listOfThings = pawn.Map.listerBuildings.AllColonistBuildingsOfType<Building_Genetron>().ToList();
                            if (listOfThings.Count > 0)
                            {
                                Building_Genetron genetron = listOfThings.RandomElement();

                                if (CheckJobPossible(pawn, genetron))
                                {

                                    var faction = Find.FactionManager.RandomEnemyFaction(allowNonHumanlike: false);
                                    pawn.SetFaction(faction);
                                    ReturnJob(pawn, genetron);
                                    Find.LetterStack.ReceiveLetter("VQE_TraitorLabel".Translate(pawn.Named("PAWN")).AdjustedFor(pawn), "VQE_TraitorLetter".Translate(pawn.Named("PAWN")).AdjustedFor(pawn), LetterDefOf.ThreatBig, new TargetInfo(pawn.Position, pawn.Map, false));
                                    parent.pawn.health.hediffSet.hediffs.Remove(parent);

                                }

                            }

                        }
                        else
                        {
                            DoIdeoSchism();
                            parent.pawn.health.hediffSet.hediffs.Remove(parent);
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
            if (DebugSettings.ShowDevGizmos)
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

        public void DoIdeoSchism()
        {
            Ideo oldIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
            var newIdeo = IdeoGenerator.MakeIdeo(oldIdeo.foundation.def);
            oldIdeo.CopyTo(newIdeo);
            var parms = new IdeoGenerationParms(Faction.OfPlayer.def);
            newIdeo.memes.Add(InternalDefOf.VQE_Technophobia);
            newIdeo.foundation.GenerateTextSymbols();
            newIdeo.foundation.RandomizePrecepts(init: false, parms);
            newIdeo.foundation.GenerateLeaderTitle();
            newIdeo.foundation.RandomizeIcon();
            newIdeo.foundation.InitPrecepts(parms);
            newIdeo.RecachePrecepts();
            newIdeo.foundation.ideo.RegenerateDescription(force: true);
            newIdeo.thingStyleCategories = new List<ThingStyleCategoryWithPriority>();
            foreach (var cat in oldIdeo.thingStyleCategories)
            {
                newIdeo.thingStyleCategories.Add(cat);
            }
            newIdeo.style.ResetStylesForThingDef();
            if (oldIdeo.Fluid)
            {
                newIdeo.Fluid = true;
                oldIdeo.development.CopyTo(newIdeo.development);
            }

            this.parent.pawn.ideo.SetIdeo(newIdeo);



            List<Pawn> convertedPawns = new List<Pawn>();

            Precept_Role precept_role = Faction.OfPlayer.ideos.PrimaryIdeo?.GetPrecept(PreceptDefOf.IdeoRole_Leader) as Precept_Role;
            Pawn leader = precept_role?.ChosenPawnSingle();

            foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists)
            {
                if (pawn != null && pawn.relations.OpinionOf(this.parent.pawn) > 35 && pawn != leader)
                {
                   
                    convertedPawns.Add(pawn);
                }
            }
            if (!convertedPawns.Any())
            {
                convertedPawns.Add(PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.RandomElement());
            }

            foreach(Pawn pawnToConvert in convertedPawns)
            {
                pawnToConvert.ideo.SetIdeo(newIdeo);
            }
            foreach(MemeDef meme in memesToRemove)
            {
                if(meme!=null && newIdeo.HasMeme(meme))
                {
                    newIdeo.memes.Remove(meme);
                }

            }
            Find.IdeoManager.Add(newIdeo);
            Faction.OfPlayer.ideos.Notify_ColonistChangedIdeo();

            Find.LetterStack.ReceiveLetter("VQE_TraitorSchism".Translate(this.parent.pawn.NameShortColored),
                "VQE_TraitorSchismDesc".Translate(this.parent.pawn.NameShortColored, newIdeo.name, this.parent.pawn.NameShortColored+", "+convertedPawns.ToStringSafeEnumerable()),LetterDefOf.NegativeEvent, this.parent.pawn);
        }
    }
}