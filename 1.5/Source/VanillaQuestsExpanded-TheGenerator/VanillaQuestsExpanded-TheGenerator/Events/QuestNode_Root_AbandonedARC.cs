using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
	public class QuestNode_Root_AbandonedARC : QuestNode_Site
	{
        public override SitePartDef QuestSite => InternalDefOf.VQE_Quest2Site;
		public QuestNode_Root_AbandonedARC()
		{
		}

		protected override void RunInt()
		{
			if (!PrepareQuest(out Quest quest, out Slate slate, out Map map, out float points, out int tile))
			{
				Log.Error("Failed to find a suitable site tile for the Ancient ARC quest.");
				return;
			}
			if (!ManhunterPackGenStepUtility.TryGetAnimalsKind(points, tile, out var animalKind))
			{
				Log.Error("Failed to find manhunter kind for the Abandoned ARC quest.");
				return;
			}
			List<Pawn> list = AggressiveAnimalIncidentUtility.GenerateAnimals(animalKind, tile, points);
			slate.Set("AnimalPlural", animalKind.GetLabelPlural());
			slate.Set("Number", list.Count());
			slate.Set("Animals", list);
			slate.Set("animalKind", animalKind);
			slate.Set("threatPoints", points);

			var site = GenerateSite(quest, slate, points, tile, null, out string siteMapGeneratedSignal);

			string siteMoreThanHalfObjectsDestroyed = QuestGenUtility.HardcodedSignalWithQuestID("site.MoreThanHalfObjectsDestroyed");
			string siteStudiedAll = QuestGenUtility.HardcodedSignalWithQuestID("site.StudiedAll");
			string siteObjectDestroyed = QuestGenUtility.HardcodedSignalWithQuestID("site.ObjectDestroyed");

			quest.AddPart(new QuestPart_Root_AbandonedARC(site, siteMapGeneratedSignal, siteStudiedAll, 
			siteMoreThanHalfObjectsDestroyed, siteObjectDestroyed));

			quest.SignalPassActivable(delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, siteMoreThanHalfObjectsDestroyed, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
			}, null, null, null, null, siteMoreThanHalfObjectsDestroyed);

			quest.SignalPass(delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
			}, siteStudiedAll);
		}
	}
}
