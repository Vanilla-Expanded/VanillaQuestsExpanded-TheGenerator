using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class CompProperties_RefuelableWithOverdrive : CompProperties_Refuelable
    {
        public bool isNuclear = false;
        public bool uraniumLevelConfigurable = false;
        public bool showUraniumGizmo = false;

        public CompProperties_RefuelableWithOverdrive()
        {
            compClass = typeof(CompRefuelableWithOverdrive);
        }


    }
}
