using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
	public abstract class QuestNode_Site : QuestNode
	{
		protected bool TryFindSiteTile(out int tile, Predicate<int> extraValidator = null)
		{
			var tiles = Find.World.tilesInRandomOrder.Tiles.Where((int x) => (extraValidator == null || extraValidator(x))
			&& IsValidTile(x));
			if (tiles.TryRandomElement(out tile))
			{
				return true;
			}
			tile = -1;
			return false;
		}

		public static bool IsValidTile(int tile)
		{
			Tile tile2 = Find.WorldGrid[tile];
			if (!tile2.biome.canBuildBase)
			{
				return false;
			}
			if (!tile2.biome.implemented)
			{
				return false;
			}
			if (tile2.hilliness == Hilliness.Impassable)
			{
				return false;
			}
			if (Find.WorldObjects.AnyMapParentAt(tile) || Current.Game.FindMap(tile) != null 
			|| Find.WorldObjects.AnyWorldObjectOfDefAt(WorldObjectDefOf.AbandonedSettlement, tile))
			{
				return false;
			}
			return true;
		}

		protected override bool TestRunInt(Slate slate)
		{
			return TryFindSiteTile(out _);
		}

		protected Site GenerateSite(SitePartDef sitePart, Quest quest, Slate slate, float points,
			int tile, Faction parentFaction, out string siteMapGeneratedSignal)
		{
			SitePartParams sitePartParams = new SitePartParams
			{
				points = points,
				threatPoints = points
			};

			Site site = QuestGen_Sites.GenerateSite(new List<SitePartDefWithParams>
			{
				new SitePartDefWithParams(sitePart, sitePartParams)
			}, tile, parentFaction);

			site.doorsAlwaysOpenForPlayerPawns = true;
			slate.Set("site", site);
			quest.SpawnWorldObject(site);

			string siteMapRemovedSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			siteMapGeneratedSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");

			quest.SignalPassActivable(delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
			}, siteMapGeneratedSignal, siteMapRemovedSignal);

			return site;
		}

		protected Faction CreateFaction(Quest quest, Slate slate, FactionDef factionDef)
		{
			FactionGeneratorParms parms = new FactionGeneratorParms(factionDef, default(IdeoGenerationParms), true);
			parms.ideoGenerationParms = new IdeoGenerationParms(parms.factionDef);
			Faction parentFaction = FactionGenerator.NewGeneratedFaction(parms);
			parentFaction.temporary = true;
			parentFaction.factionHostileOnHarmByPlayer = true;
			parentFaction.neverFlee = true;
			Find.FactionManager.Add(parentFaction);
			quest.ReserveFaction(parentFaction);
			slate.Set("parentFaction", parentFaction);
			slate.Set("siteFaction", parentFaction);
			return parentFaction;
		}

		protected bool PrepareQuest(out Quest quest, out Slate slate, out Map map, out float points, 
		out int tile, Predicate<int> extraValidator = null)
		{
			quest = QuestGen.quest;
			slate = QuestGen.slate;
			map = QuestGen_Get.GetMap();
			QuestGenUtility.RunAdjustPointsForDistantFight();
			points = slate.Get("points", 0f);
			slate.Set("playerFaction", Faction.OfPlayer);
			slate.Set("inventor", Genetron_GameComponent.Instance.inventor);
			slate.Set("map", map);
			if (!TryFindSiteTile(out tile, extraValidator))
			{
				return false;
			}
			return true;
		}

		protected string GenerateTerminal(Quest quest, Slate slate, Site site)
		{
			string terminalDestroyedSignal = QuestGenUtility.HardcodedSignalWithQuestID("terminal.Destroyed");
			string terminalHackedSignal = QuestGenUtility.HardcodedSignalWithQuestID("terminal.Studied");
			string terminalHackingStartedSignal = QuestGenUtility.HardcodedSignalWithQuestID("terminal.StudyStarted");

			// Handle the Ancient ARC terminal
			Building_Genetron_Studiable terminal = site.parts[0].things
				.FirstOrDefault(t => t.def == InternalDefOf.VQE_AncientResearchTerminal) as Building_Genetron_Studiable;
			slate.Set("terminal", terminal);
			terminal.studyFinishedSignal = terminalHackedSignal;
			terminal.studyStartedSignal = terminalHackingStartedSignal;
			// Set faction to hidden until the terminal is interacted with

			// Signals for when the terminal is destroyed or hacked
			quest.SignalPassActivable(delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, terminalDestroyedSignal, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
			}, null, null, null, null, terminalDestroyedSignal);

			// When the terminal is hacked or studied, the quest progresses
			quest.SignalPass(delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
			}, terminalHackedSignal);

			return terminalHackingStartedSignal;
		}
	}
}
