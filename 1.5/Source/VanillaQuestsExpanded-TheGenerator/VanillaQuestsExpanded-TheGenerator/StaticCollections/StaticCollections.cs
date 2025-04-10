﻿
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Net;



namespace VanillaQuestsExpandedTheGenerator
{
    [StaticConstructorOnStartup]
    public static class StaticCollections
    {
        

        public static Dictionary<Map,int> ARClevelInMap = new Dictionary<Map,int>();
        public static Dictionary<Map, float> ARCmaintenanceInMap = new Dictionary<Map, float>();

       
     
        public static HashSet<BuildableDef> genetrons = new HashSet<BuildableDef>() { InternalDefOf.VQE_Genetron_Basic, InternalDefOf.VQE_Genetron_WoodFired,
            InternalDefOf.VQE_Genetron_WoodFueled, InternalDefOf.VQE_Genetron_WoodPowered, InternalDefOf.VQE_Genetron_WoodBlasting,
        InternalDefOf.VQE_Genetron_ChemfuelPowered, InternalDefOf.VQE_Genetron_ChemfuelBoosted, InternalDefOf.VQE_Genetron_ChemfuelCharged,
        InternalDefOf.VQE_Genetron_ChemfuelFortified,InternalDefOf.VQE_Genetron_Geothermal,InternalDefOf.VQE_Genetron_SteamPowered,
        InternalDefOf.VQE_Genetron_ThermalVent, InternalDefOf.VQE_Genetron_HeatPowered,InternalDefOf.VQE_Genetron_UraniumPowered,
        InternalDefOf.VQE_Genetron_Nuclear,InternalDefOf.VQE_Genetron_Isotopic,InternalDefOf.VQE_Genetron_Atomic};

        public static HashSet<BuildableDef> geothermalGenetrons = new HashSet<BuildableDef>() { InternalDefOf.VQE_Genetron_SteamPowered,
        InternalDefOf.VQE_Genetron_ThermalVent,InternalDefOf.VQE_Genetron_HeatPowered};

        public static HashSet<BuildableDef> nuclearGenetrons = new HashSet<BuildableDef>() { InternalDefOf.VQE_Genetron_UraniumPowered,
        InternalDefOf.VQE_Genetron_Nuclear,InternalDefOf.VQE_Genetron_Isotopic,InternalDefOf.VQE_Genetron_Atomic };

        public static HashSet<BackstoryDef> basicBackstories = new HashSet<BackstoryDef>() { InternalDefOf.VQE_AncientEngineer01,InternalDefOf.VQE_PowerArchitect02,
            InternalDefOf.VQE_ControlSystemsEngineer03,InternalDefOf.VQE_FieldRepairTech04,InternalDefOf.VQE_SafetyInspector05,InternalDefOf.VQE_ChildhoodInventorGenetron, InternalDefOf.VQE_InventorGenetronAdulthood};

        public static HashSet<BackstoryDef> advancedBackstories = new HashSet<BackstoryDef>() { InternalDefOf.VQE_ChildhoodInventorGenetron, InternalDefOf.VQE_InventorGenetronAdulthood};

        public static HashSet<HediffDef> studiedHediffs = new HashSet<HediffDef>() { InternalDefOf.VQE_StudiedAncientGenetron,
        InternalDefOf.VQE_StudiedAncientGeothermalGenetron,InternalDefOf.VQE_StudiedAncientNuclearGenetron};

        public static void SetArcLevelInMap(Map map, int level)
        {
            ARClevelInMap[map] = level;

        }

        public static void SetArcMaintenanceInMap(Map map, float maintenance)
        {
            ARCmaintenanceInMap[map] = maintenance;

        }

    }
}
