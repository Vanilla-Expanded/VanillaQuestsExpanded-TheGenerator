using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{

    [DefOf]
    public static class InternalDefOf
    {
        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }

        public static ThingDef VQE_GenetronComponent;

        public static ThingDef VQE_GenetronJunk;
        public static ThingDef VQE_ChunkProjectile;
        public static ThingDef Filth_Ash;
        public static ThingDef VQE_EmptyConstructionPallet;

        public static ThingDef VQE_Genetron_Basic;
        //Wood
        public static ThingDef VQE_Genetron_WoodFired;
        public static ThingDef VQE_Genetron_WoodFueled;
        public static ThingDef VQE_Genetron_WoodPowered;
        public static ThingDef VQE_Genetron_WoodBlasting;
        //Chemfuel
        public static ThingDef VQE_Genetron_ChemfuelPowered;
        public static ThingDef VQE_Genetron_ChemfuelBoosted;
        public static ThingDef VQE_Genetron_ChemfuelCharged;
        public static ThingDef VQE_Genetron_ChemfuelFortified;
        //Geothermal
        public static ThingDef VQE_Genetron_Geothermal;
        public static ThingDef VQE_Genetron_SteamPowered;
        public static ThingDef VQE_Genetron_ThermalVent;
        public static ThingDef VQE_Genetron_HeatPowered;
        //Nuclear
        public static ThingDef VQE_Genetron_UraniumPowered;
        public static ThingDef VQE_Genetron_Nuclear;
        public static ThingDef VQE_Genetron_Isotopic;
        public static ThingDef VQE_Genetron_Atomic;

        //Ruins
        public static ThingDef VQE_NuclearGenetronHusk;

        public static HediffDef VQE_StudiedAncientGenetron;
        public static HediffDef VQE_StudiedAncientGeothermalGenetron;
        public static HediffDef VQE_StudiedAncientNuclearGenetron;

        public static JobDef VQE_FixCriticallyBrokenDownBuilding;
        public static JobDef VQE_MaintainGenetron;
        public static JobDef VQE_StudyGenetron;

        public static StatDef VQE_GenetronMaintenanceLoss;

        public static ResearchProjectDef BiofuelRefining;
        public static ResearchProjectDef GeothermalPower;
        public static ResearchProjectDef ShipReactor;

        public static SoundDef VQE_MeltdownExplosion;

        public static BackstoryDef VQE_AncientEngineer01;
        public static BackstoryDef VQE_PowerArchitect02;
        public static BackstoryDef VQE_ControlSystemsEngineer03;
        public static BackstoryDef VQE_FieldRepairTech04;
        public static BackstoryDef VQE_SafetyInspector05;
        public static BackstoryDef VQE_ChildhoodInventorGenetron;
        public static BackstoryDef VQE_InventorGenetronAdulthood;

        public static SitePartDef VQE_Quest3Site;
        public static ThingDef VQE_AncientResearchTerminal;
        public static PawnKindDef VQE_Inventor;
        public static RulePackDef VQE_InventorMaleNames, VQE_InventorFemaleNames, VQE_InventorLastNames;
    }
}