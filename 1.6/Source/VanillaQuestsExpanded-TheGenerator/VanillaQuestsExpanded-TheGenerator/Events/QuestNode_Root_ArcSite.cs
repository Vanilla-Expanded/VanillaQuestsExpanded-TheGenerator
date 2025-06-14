using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace VanillaQuestsExpandedTheGenerator
{

	public class QuestNode_Root_ArcSite : QuestNode_Site
	{
		public override SitePartDef QuestSite => InternalDefOf.VQE_Quest5Site;
		public QuestNode_Root_ArcSite()
		{

		}

		protected override void RunInt()
		{
			if (!PrepareQuest(out Quest quest, out Slate slate, out Map map, out float points, out int tile))
			{
				Log.Error("Failed to find a suitable site tile for the Ancient ARC quest.");
				return;
			}

			var parentFaction = CreateFaction(quest, slate, FactionDefOf.Pirate);
			Site site = GenerateSite(quest, slate, points, tile, parentFaction,
			out string siteMapGeneratedSignal);
			var asker = slate.Get<Pawn>("asker");
			if (asker is null)
			{
				var allEnemiesDestroyed = QuestGenUtility.HardcodedSignalWithQuestID("site.AllEnemiesDefeated");
				GiveRewards(quest, new RewardsGeneratorParams
				{
					rewardValue = 100f,
				}, allEnemiesDestroyed, addCampLootReward: true);
			}
		}
		public static QuestPart_Choice GiveRewards(Quest quest, RewardsGeneratorParams parms, string inSignal = null, string customLetterLabel = null, string customLetterText = null, RulePack customLetterLabelRules = null, RulePack customLetterTextRules = null, bool? useDifficultyFactor = null, Action runIfChosenPawnSignalUsed = null, int? variants = null, bool addCampLootReward = false, Pawn asker = null, bool addShuttleLootReward = false, bool addPossibleFutureReward = false, float? overridePopulationIntent = null)
		{
			try
			{
				Slate slate = QuestGen.slate;
				RewardsGeneratorParams parmsResolved = parms;
				Slate.VarRestoreInfo restoreInfo = slate.GetRestoreInfo("inSignal");
				if (inSignal.NullOrEmpty())
				{
					inSignal = slate.Get<string>("inSignal");
				}
				else
				{
					slate.Set("inSignal", QuestGenUtility.HardcodedSignalWithQuestID(inSignal));
				}
				try
				{
					QuestPart_Choice questPart_Choice = new QuestPart_Choice();
					questPart_Choice.inSignalChoiceUsed = slate.Get<string>("inSignal");
					QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
					questPart_Choice.choices.Add(choice);
					for (int j = 0; j < questPart_Choice.choices.Count; j++)
					{
						questPart_Choice.choices[j].rewards.Add(new Reward_CampLoot());
					}
					quest.AddPart(questPart_Choice);
					return questPart_Choice;
				}
				finally
				{
					slate.Restore(restoreInfo);
				}
			}
			finally
			{
			}
		}
	}
}
