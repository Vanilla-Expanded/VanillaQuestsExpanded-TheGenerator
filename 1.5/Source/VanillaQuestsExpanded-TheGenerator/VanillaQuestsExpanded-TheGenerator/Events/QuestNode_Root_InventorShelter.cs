using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
	public class QuestNode_Root_InventorShelter : QuestNode_Site
	{
		public override SitePartDef QuestSite => InternalDefOf.VQE_Quest4Site;

		public QuestNode_Root_InventorShelter()
		{

		}

		protected override void RunInt()
		{
			if (Genetron_GameComponent.Instance.inventorSpawned)
			{
				return;
			}
			bool seaIcePresent = Find.WorldGrid.tiles.Any(x => x.biome == BiomeDefOf.SeaIce);
			if (!PrepareQuest(out Quest quest, out Slate slate, out Map map, out float points, out int tile, delegate (int x)
			{
				if (seaIcePresent)
				{
					return Find.WorldGrid[x].biome == BiomeDefOf.SeaIce;
				}
				return true;
			}))
			{
				Log.Error("Failed to find a suitable site tile for the Ancient ARC quest.");
				return;
			}
			
			string siteInventorReleased = QuestGenUtility.HardcodedSignalWithQuestID("site.InventorReleased");
			quest.SignalPass(delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
			}, siteInventorReleased);
			slate.Set("Inventor_LastName", (Genetron_GameComponent.Instance.inventor.Name as NameTriple).Last);
			Site site = GenerateSite(quest, slate, points, tile, null, out string siteMapGeneratedSignal);
		}
	}
}
