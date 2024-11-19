﻿
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;



namespace VanillaQuestsExpandedTheGenerator
{
    [StaticConstructorOnStartup]
    public static class StaticCollections
    {

        static StaticCollections()
        {

            foreach (HiddenDesignatorsDef hiddenDesignatorDef in DefDatabase<HiddenDesignatorsDef>.AllDefsListForReading)
            {
                foreach (BuildableDef thing in hiddenDesignatorDef.hiddenDesignators)
                {

                    hidden_designators.Add(thing);
                }
            }


        }

        // A list of designators that shouldn't appear on the architect menu.
        public static HashSet<BuildableDef> hidden_designators = new HashSet<BuildableDef>();
     
        public static HashSet<BuildableDef> genetrons = new HashSet<BuildableDef>() { InternalDefOf.VQE_Genetron_Basic, InternalDefOf.VQE_Genetron_WoodFired,
            InternalDefOf.VQE_Genetron_WoodFueled, InternalDefOf.VQE_Genetron_WoodPowered, InternalDefOf.VQE_Genetron_WoodBlasting,
        InternalDefOf.VQE_Genetron_ChemfuelPowered, InternalDefOf.VQE_Genetron_ChemfuelBoosted, InternalDefOf.VQE_Genetron_ChemfuelCharged,
        InternalDefOf.VQE_Genetron_ChemfuelFortified};
   

    }
}
