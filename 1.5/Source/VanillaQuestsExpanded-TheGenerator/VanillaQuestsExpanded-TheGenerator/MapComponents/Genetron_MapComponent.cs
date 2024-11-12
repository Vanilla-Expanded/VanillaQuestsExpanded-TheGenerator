using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using RimWorld.Planet;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Genetron_MapComponent : MapComponent
    {    

        public HashSet<Thing> refuelables_InMap = new HashSet<Thing>();
        public HashSet<Thing> repairables_InMap = new HashSet<Thing>();

        public Genetron_MapComponent(Map map) : base(map)
        {
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref this.repairables_InMap, "repairables_InMap", LookMode.Reference);
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
        }

        public override void MapGenerated()
        {
            base.MapGenerated();
        }

        public void AddRefuelableToMap(Thing thing)
        {
            if (!refuelables_InMap.Contains(thing))
            {
                refuelables_InMap.Add(thing);
            }
        }

        public void RemoveRefuelableFromMap(Thing thing)
        {
            if (refuelables_InMap.Contains(thing))
            {
                refuelables_InMap.Remove(thing);
            }

        }

        public void AddRepairableToMap(Thing thing)
        {
            if (!repairables_InMap.Contains(thing))
            {
                repairables_InMap.Add(thing);
            }
        }

        public void RemoveRepairableFromMap(Thing thing)
        {
            if (repairables_InMap.Contains(thing))
            {
                repairables_InMap.Remove(thing);
            }

        }


    }


}
