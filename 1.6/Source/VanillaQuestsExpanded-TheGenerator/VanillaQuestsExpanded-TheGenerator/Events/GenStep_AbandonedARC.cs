using Verse;
using RimWorld;
using KCSG;
using System.Collections.Generic;
using VEF.Buildings;
namespace VanillaQuestsExpandedTheGenerator
{
	public class GenStep_AbandonedARC : GenStep_CustomStructureGen
	{
		public FloatRange defaultPointsRange = new FloatRange(300f, 500f);
		private int MinRoomCells = 225;
		public override int SeedPart => 457293335;
		public override void PostGenerate(CellRect rect, Map map, GenStepParams parms)
		{
			base.PostGenerate(rect, map, parms);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors).WithFenceblocked(forceFenceblocked: true);
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) 
			&& !x.Fogged(map) && map.reachability.CanReachMapEdge(x, traverseParams) 
			&& x.GetRoom(map).CellCount >= MinRoomCells, map, out var result))
			{
				float points = ((parms.sitePart != null) ? parms.sitePart.parms.threatPoints : defaultPointsRange.RandomInRange);
				PawnKindDef animalKind;
				if (parms.sitePart != null && parms.sitePart.parms.animalKind != null)
				{
					animalKind = parms.sitePart.parms.animalKind;
				}
				else if (!ManhunterPackGenStepUtility.TryGetAnimalsKind(points, map.Tile, out animalKind))
				{
					return;
				}
				List<Pawn> list = AggressiveAnimalIncidentUtility.GenerateAnimals(animalKind, map.Tile, points);
				for (int i = 0; i < list.Count; i++)
				{
					IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(result, map, 10);
					GenSpawn.Spawn(list[i], loc, map, Rot4.Random);
					list[i].health.AddHediff(HediffDefOf.Scaria);
					list[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);
				}
			}
			
			foreach (var studiables in map.listerThings.GetThingsOfType<Building_Genetron_Studiable>())
			{
				studiables.GetComp<CompBouncingArrow>().doBouncingArrow = true;
			}
		}
	}
}
