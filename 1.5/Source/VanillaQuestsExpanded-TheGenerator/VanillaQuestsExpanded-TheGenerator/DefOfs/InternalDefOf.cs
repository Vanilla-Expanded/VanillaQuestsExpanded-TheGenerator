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

        public static ThingDef VQE_Genetron_Basic;
        public static ThingDef VQE_Genetron_WoodFired;
        public static ThingDef VQE_Genetron_WoodFueled;
        public static ThingDef VQE_Genetron_WoodPowered;
        public static ThingDef VQE_Genetron_WoodBlasting;
        public static ThingDef VQE_Genetron_ChemfuelPowered;

        public static HediffDef VQE_StudiedAncientGenetron;

        public static JobDef VQE_FixCriticallyBrokenDownBuilding;
        public static JobDef VQE_MaintainGenetron;

        public static StatDef VQE_GenetronMaintenanceLoss;
    }
}