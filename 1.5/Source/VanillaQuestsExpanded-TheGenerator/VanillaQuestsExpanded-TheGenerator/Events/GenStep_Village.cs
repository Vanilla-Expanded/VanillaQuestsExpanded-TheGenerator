using System;
using Verse;
using RimWorld;
using KCSG;
using Verse.AI.Group;
using RimWorld.BaseGen;

namespace VanillaQuestsExpandedTheGenerator
{
	public class GenStep_Village : GenStep_CustomStructureGen
	{
		public override void PostGenerate(CellRect rect, Map map, GenStepParams parms)
		{
			base.PostGenerate(rect, map, parms);
			var faction = map.ParentFaction;
			Lord singlePawnLord = LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, rect.CenterCell,
				true), map);
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors);
			ResolveParams resolveParams = default;
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			resolveParams.singlePawnLord = singlePawnLord;
			resolveParams.pawnGroupKindDef = PawnGroupKindDefOf.Settlement;
			resolveParams.singlePawnSpawnCellExtraPredicate = ((Predicate<IntVec3>)((IntVec3 x) => map.reachability.CanReachMapEdge(x, traverseParms)));
			resolveParams.pawnGroupMakerParams = new PawnGroupMakerParms();
			resolveParams.pawnGroupMakerParams.tile = map.Tile;
			resolveParams.pawnGroupMakerParams.faction = faction;
			resolveParams.pawnGroupMakerParams.inhabitants = true;
			resolveParams.settlementPawnGroupSeed = OutpostSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms);
			resolveParams.settlementPawnGroupPoints = SymbolResolver_Settlement.DefaultPawnsPoints.RandomInRange;
			resolveParams.pawnGroupMakerParams.points = resolveParams.settlementPawnGroupPoints.Value;
			BaseGen.symbolStack.Push("pawnGroup", resolveParams);
			BaseGen.globalSettings.map = map;
			BaseGen.Generate();
		}
	}
}
