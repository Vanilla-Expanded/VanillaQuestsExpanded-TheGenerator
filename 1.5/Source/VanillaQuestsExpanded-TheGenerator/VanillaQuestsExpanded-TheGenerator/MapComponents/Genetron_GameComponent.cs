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
   
    public static Genetron_GameComponent Instance;

    public Genetron_GameComponent(Game game) => Instance = this;

    public bool geothermalGenetronStudied = false;
    public bool nuclearGenetronStudied = false;

    public override void StartedNewGame()
    {
        base.StartedNewGame();
        CreateInventor();
    }

    private void CreateInventor()
    {
        inventor = PawnGenerator.GeneratePawn(InternalDefOf.VQE_Inventor);
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
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref this.nuclearGenetronStudied, "nuclearGenetronStudied", false, false);
        Scribe_Values.Look(ref this.geothermalGenetronStudied, "geothermalGenetronStudied", false, false);
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            if (inventor is null)
            {
                CreateInventor();
            }
        }
        if (inventor != null && inventor.ParentHolder is not null)
        {
            Scribe_References.Look(ref inventor, "inventor");
        }
        else
        {
            Scribe_Deep.Look(ref inventor, "inventor");
        }
    }



}