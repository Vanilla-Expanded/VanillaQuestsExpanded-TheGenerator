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
    public class QuestNode_Root_AncientARCStudy : QuestNode_Site
    {
        private const int FactionBecomesHostileAfterHours = 10;

        private const float PointsMultiplierRaid = 0.2f;

        private const float MinPointsRaid = 45f;

        public QuestNode_Root_AncientARCStudy()
        {
        }

        protected override void RunInt()
        {
            if (!PrepareQuest(out Quest quest, out Slate slate, out Map map, out float points, out int tile))
            {
                Log.Error("Failed to find a suitable site tile for the Ancient ARC quest.");
                return;
            }

            // Create a temporary tribal faction to guard the site
            var parentFaction = CreateFaction(quest, slate, FactionDefOf.TribeCivil);

            // Generate site parts and the site itself
            Site site = GenerateSite(InternalDefOf.VQE_Quest3Site, quest, slate, points, tile, parentFaction, out string siteMapGeneratedSignal);

            // Time before faction becomes hostile if overstayed
            int hostileTimerTicks = GenDate.TicksPerHour * FactionBecomesHostileAfterHours;
            site.GetComponent<TimedMakeFactionHostile>().SetupTimer(hostileTimerTicks, "VQE_FactionBecameHostileTimed".Translate(parentFaction.Named("FACTION")));

            string tribalFactionMemberArrestedSignal = QuestGenUtility.HardcodedSignalWithQuestID("parentFaction.FactionMemberArrested");

            quest.SetFactionHidden(parentFaction);

            string terminalHackingStartedSignal = GenerateTerminal(quest, slate, site);

            // Start recurring raids if violence is allowed
            if (Find.Storyteller.difficulty.allowViolentQuests)
            {
                quest.FactionRelationToPlayerChange(parentFaction, FactionRelationKind.Hostile, canSendHostilityLetter: false, terminalHackingStartedSignal);
                quest.StartRecurringRaids(site, new FloatRange(2.5f, 2.5f), GenDate.TicksPerHour, siteMapGeneratedSignal);
                quest.FactionRelationToPlayerChange(parentFaction, FactionRelationKind.Hostile, canSendHostilityLetter: true, tribalFactionMemberArrestedSignal);
            }

            // Final setup for quest-related data
            slate.Set("timer", hostileTimerTicks);
        }
    }
}
