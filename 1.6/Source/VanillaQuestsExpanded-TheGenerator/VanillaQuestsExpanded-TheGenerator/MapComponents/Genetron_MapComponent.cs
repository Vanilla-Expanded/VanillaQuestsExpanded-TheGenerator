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
        public HashSet<Thing> maintainables_InMap = new HashSet<Thing>();
        public HashSet<Thing> uraniumFueled_InMap = new HashSet<Thing>();
        public HashSet<Thing> studiables_InMap = new HashSet<Thing>();
        public int tickCounter = tickInterval;
        public const int tickInterval = 1000;

        public Genetron_MapComponent(Map map) : base(map)
        {
        }

        public override void MapComponentTick()
        {
            if (Find.IdeoManager.classicMode) return;

            tickCounter++;
            if ((tickCounter > tickInterval))
            {
                List<Building_Genetron> listOfThings = map.listerBuildings.AllColonistBuildingsOfType<Building_Genetron>()?.ToList();
                if (listOfThings.Count > 0)
                {
                   
                    IEnumerable<int> levels = listOfThings.Select(x => x.cachedDetailsExtension.ARClevel);
              
                    if (levels.Count() > 0)
                    {
                        StaticCollections.SetArcLevelInMap(map, levels.Max());
                    }
                    IEnumerable<float> maintenances = listOfThings.Where(x => x is Building_GenetronWithMaintenance)?.Select(x => ((Building_GenetronWithMaintenance)x).maintenance);
                    if (maintenances.Count() > 0)
                    {
                        StaticCollections.SetArcMaintenanceInMap(map, maintenances.Max());
                    }



                }
                else
                {
                    StaticCollections.SetArcLevelInMap(map, 0);
                    StaticCollections.SetArcMaintenanceInMap(map, 1);
                }
                                   
                tickCounter = 0;
            }

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref this.repairables_InMap, "repairables_InMap", LookMode.Reference);
            Scribe_Collections.Look(ref this.studiables_InMap, "studiables_InMap", LookMode.Reference);

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
        public void AddMaintainableToMap(Thing thing)
        {
            if (!maintainables_InMap.Contains(thing))
            {
                maintainables_InMap.Add(thing);
            }
        }

        public void RemoveMaintainableFromMap(Thing thing)
        {
            if (maintainables_InMap.Contains(thing))
            {
                maintainables_InMap.Remove(thing);
            }

        }

        public void AddUraniumFueledToMap(Thing thing)
        {
            if (!uraniumFueled_InMap.Contains(thing))
            {
                uraniumFueled_InMap.Add(thing);
            }
        }

        public void RemoveUraniumFueledFromMap(Thing thing)
        {
            if (uraniumFueled_InMap.Contains(thing))
            {
                uraniumFueled_InMap.Remove(thing);
            }

        }
        public void AddStudiablesToMap(Thing thing)
        {
            if (!studiables_InMap.Contains(thing))
            {
                studiables_InMap.Add(thing);
            }
        }

        public void RemoveStudiablesFromMap(Thing thing)
        {
            if (studiables_InMap.Contains(thing))
            {
                studiables_InMap.Remove(thing);
            }

        }

    }


}
