using HarmonyLib;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaQuestsExpandedTheGenerator;

public class Genetron_GameComponent : GameComponent
{

   
    public static Genetron_GameComponent Instance;

    public Genetron_GameComponent(Game game) => Instance = this;

    public bool geothermalGenetronStudied = false;
    public bool nuclearGenetronStudied = false;


    public override void ExposeData()
    {
        Scribe_Values.Look(ref this.nuclearGenetronStudied, "nuclearGenetronStudied", false, false);
        Scribe_Values.Look(ref this.geothermalGenetronStudied, "geothermalGenetronStudied", false, false);
    }

  

}