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

        public static ThingDef VQE_Genetron_Basic;
        public static ThingDef VQE_Genetron_WoodFired;
        public static ThingDef VQE_Genetron_WoodFueled;
        public static ThingDef VQE_Genetron_WoodPowered;

        public static HediffDef VQE_StudiedAncientGenetron;
    }
}