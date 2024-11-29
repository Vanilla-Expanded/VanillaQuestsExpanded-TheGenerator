using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{

    public class QuestNode_Root_AncientARCStudy : QuestNode
    {
        private const int MinDistanceFromColony = 2;

        private const int MaxDistanceFromColony = 99999;

        private const int FactionBecomesHostileAfterHours = 10;

        private const float PointsMultiplierRaid = 0.2f;

        private const float MinPointsRaid = 45f;

        public QuestNode_Root_AncientARCStudy()
        {
        }

        protected override void RunInt()
        {
            Quest quest = QuestGen.quest;
            Slate slate = QuestGen.slate;
            Map map = QuestGen_Get.GetMap();
            QuestGenUtility.RunAdjustPointsForDistantFight();
            float points = slate.Get("points", 0f);
            slate.Set("playerFaction", Faction.OfPlayer);
            slate.Set("inventor", Genetron_GameComponent.Instance.inventor);
            // Try to find a site tile within range
            if (!TryFindSiteTile(out var tile))
            {
                Log.Error("Failed to find a suitable site tile for the Ancient ARC quest.");
                return;
            }

            // Create a temporary tribal faction to guard the site
            FactionGeneratorParms parms = new FactionGeneratorParms(FactionDefOf.TribeCivil, default(IdeoGenerationParms), true);
            parms.ideoGenerationParms = new IdeoGenerationParms(parms.factionDef);
            Faction tribalFaction = FactionGenerator.NewGeneratedFaction(parms);
            tribalFaction.temporary = true;
            tribalFaction.factionHostileOnHarmByPlayer = true;
            tribalFaction.neverFlee = true;
            Find.FactionManager.Add(tribalFaction);
            quest.ReserveFaction(tribalFaction);

            // Generate site parts and the site itself
            SitePartParams sitePartParams = new SitePartParams
            {
                points = points,
                threatPoints = points
            };

            Site site = QuestGen_Sites.GenerateSite(new List<SitePartDefWithParams>
            {
                new SitePartDefWithParams(InternalDefOf.VQE_Quest3Site, sitePartParams)
            }, tile, tribalFaction);

            site.doorsAlwaysOpenForPlayerPawns = true;
            slate.Set("site", site);
            quest.SpawnWorldObject(site);

            // Time before faction becomes hostile if overstayed
            int hostileTimerTicks = GenDate.TicksPerHour * FactionBecomesHostileAfterHours;
            site.GetComponent<TimedMakeFactionHostile>().SetupTimer(hostileTimerTicks, "VQE_FactionBecameHostileTimed".Translate(tribalFaction.Named("FACTION")));

            // Signals for terminal actions and site events
            string terminalDestroyedSignal = QuestGenUtility.HardcodedSignalWithQuestID("terminal.Destroyed");
            string terminalHackedSignal = QuestGenUtility.HardcodedSignalWithQuestID("terminal.Studied");
            string terminalHackingStartedSignal = QuestGenUtility.HardcodedSignalWithQuestID("terminal.StudyStarted");
            string siteMapRemovedSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
            string siteMapGeneratedSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
            string tribalFactionMemberArrestedSignal = QuestGenUtility.HardcodedSignalWithQuestID("tribalFaction.FactionMemberArrested");

            // Handle the Ancient ARC terminal
            Building_Genetron_Studiable terminal = site.parts[0].things
                .FirstOrDefault(t => t.def == InternalDefOf.VQE_AncientResearchTerminal) as Building_Genetron_Studiable;
            slate.Set("terminal", terminal);
            terminal.studyFinishedSignal = terminalHackedSignal;
            terminal.studyStartedSignal = terminalHackingStartedSignal;
            // Set faction to hidden until the terminal is interacted with
            quest.SetFactionHidden(tribalFaction);

            // Signals for when the terminal is destroyed or hacked
            quest.SignalPassActivable(delegate
            {
                quest.End(QuestEndOutcome.Fail, 0, null, terminalDestroyedSignal, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
            }, null, null, null, null, terminalDestroyedSignal);

            // When the terminal is hacked or studied, the quest progresses
            quest.SignalPassAll(delegate
            {
                quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
            }, new List<string> { terminalHackedSignal, siteMapRemovedSignal });

            // Handle overstaying or hostility trigger
            quest.SignalPassActivable(delegate
            {
                quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
            }, siteMapGeneratedSignal, siteMapRemovedSignal);

            // Start recurring raids if violence is allowed
            if (Find.Storyteller.difficulty.allowViolentQuests)
            {
                quest.FactionRelationToPlayerChange(tribalFaction, FactionRelationKind.Hostile, canSendHostilityLetter: false, terminalHackingStartedSignal);
                quest.StartRecurringRaids(site, new FloatRange(2.5f, 2.5f), GenDate.TicksPerHour, siteMapGeneratedSignal);
                quest.FactionRelationToPlayerChange(tribalFaction, FactionRelationKind.Hostile, canSendHostilityLetter: true, tribalFactionMemberArrestedSignal);
            }

            // Final setup for quest-related data
            slate.Set("map", map);
            slate.Set("timer", hostileTimerTicks);
            slate.Set("tribalFaction", tribalFaction);
        }

        private bool TryFindSiteTile(out int tile)
        {
            return TileFinder.TryFindNewSiteTile(out tile, MinDistanceFromColony, MaxDistanceFromColony);
        }

        protected override bool TestRunInt(Slate slate)
        {
            return TryFindSiteTile(out _);
        }
    }
}
