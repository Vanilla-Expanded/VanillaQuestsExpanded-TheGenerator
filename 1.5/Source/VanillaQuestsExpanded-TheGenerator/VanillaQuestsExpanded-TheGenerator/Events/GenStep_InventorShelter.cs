using Verse;
using KCSG;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace VanillaQuestsExpandedTheGenerator
{
	public class GenStep_InventorShelter : GenStep_CustomStructureGen
	{
		public override void PostGenerate(CellRect rect, Map map, GenStepParams parms)
		{
			base.PostGenerate(rect, map, parms);
			var casket = map.listerThings.GetThingsOfType<Building_Genetron_AncientCryptosleepPod>().First();
			casket.InnerContainer.ClearAndDestroyContents();
			casket.TryAcceptThing(Genetron_GameComponent.Instance.inventor);
			Genetron_GameComponent.Instance.inventorSpawned = true;
			foreach (var turret in map.listerThings.AllThings.OfType<Building_Turret>())
			{
				turret.SetFaction(Faction.OfAncientsHostile);
			}
			var gameCondition = GameConditionMaker.MakeConditionPermanent(InternalDefOf.VQE_UnnaturalColdSnap);
			gameCondition.startTick = Find.TickManager.TicksGame - gameCondition.TransitionTicks;
			map.gameConditionManager.RegisterCondition(gameCondition);
			Find.World.GetComponent<TileTemperaturesComp>().ClearCaches();
			map.weatherDecider.StartNextWeather();
			map.weatherManager.curWeatherAge = GenDate.TicksPerDay;
		}
	}
}
