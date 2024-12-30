using HarmonyLib;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace VanillaQuestsExpandedTheGenerator;

public class Genetron_GameComponent : GameComponent
{
	public Pawn inventor;
	public bool inventorSpawned;
	public static Genetron_GameComponent Instance;

	public Genetron_GameComponent(Game game) => Instance = this;

	public bool anyGenetronStudied = false;
	public bool geothermalGenetronStudied = false;
	public bool nuclearGenetronStudied = false;

	public HashSet<Thing> supressedGeysers = new HashSet<Thing>();

	public override void StartedNewGame()
	{
		base.StartedNewGame();
		CreateInventor();
	}

	private void CreateInventor()
	{
		inventor = PawnGenerator.GeneratePawn(InternalDefOf.VQE_Inventor);
		foreach (ThingDef thingDef in inventor.kindDef.apparelRequired)
		{
			var newApparel = ThingMaker.MakeThing(thingDef, GenStuff.RandomStuffFor(thingDef)) as Apparel;
			inventor.apparel.Wear(newApparel);
		}
		GrammarRequest firstNameReq = default;
		if (inventor.gender == Gender.Male)
		{
			firstNameReq.Includes.Add(InternalDefOf.VQE_InventorMaleNames);
		}
		else
		{
			firstNameReq.Includes.Add(InternalDefOf.VQE_InventorFemaleNames);
		}
		var firstName = GrammarResolver.Resolve("r_first_name", firstNameReq);
		GrammarRequest lastNameReq = default;
		lastNameReq.Includes.Add(InternalDefOf.VQE_InventorLastNames);
		var lastName = GrammarResolver.Resolve("r_last_name", lastNameReq);
		var name = new NameTriple(firstName, "", lastName);
		inventor.Name = name;
		inventor.abilities.GetAbility(InternalDefOf.VQE_CraftAnARCComponent);
	}

	public override void ExposeData()
	{
		Scribe_Values.Look(ref this.anyGenetronStudied, "anyGenetronStudied", false, false);
		Scribe_Values.Look(ref this.nuclearGenetronStudied, "nuclearGenetronStudied", false, false);
		Scribe_Values.Look(ref this.geothermalGenetronStudied, "geothermalGenetronStudied", false, false);
		Scribe_Collections.Look(ref this.supressedGeysers, "supressedGeysers",LookMode.Reference);

		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (inventor is null)
			{
				CreateInventor();
			}
		}
		Scribe_Values.Look(ref inventorSpawned, "inventorSpawned", false, false);
		if (inventorSpawned)
		{
			Scribe_References.Look(ref inventor, "inventor");
		}
		else
		{
			Scribe_Deep.Look(ref inventor, "inventor");
		}
	}

	public void AddGeyserToSuppressed(Thing geyser) {
		if (!supressedGeysers.Contains(geyser))
		{
			supressedGeysers.Add(geyser);
		}
	
	}
	public void RemoveGeyserFromSuppressed(Thing geyser)
	{
		if (supressedGeysers.Contains(geyser))
		{
			supressedGeysers.Remove(geyser);
		}

	}



}