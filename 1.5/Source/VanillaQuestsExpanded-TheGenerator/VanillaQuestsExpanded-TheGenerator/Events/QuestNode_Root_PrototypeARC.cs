using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
	public class QuestNode_Root_PrototypeARC : QuestNode_Site
	{
		public QuestNode_Root_PrototypeARC()
		{

		}

		protected override void RunInt()
		{
			if (!PrepareQuest(out Quest quest, out Slate slate, out Map map, out float points, out int tile))
			{
				Log.Error("Failed to find a suitable site tile for the Ancient ARC quest.");
				return;
			}
			Site site = GenerateSite(InternalDefOf.VQE_Quest1Site, quest, slate, points, tile, null, out string siteMapGeneratedSignal);
			GenerateTerminal(quest, slate, site);
		}
	}
}
