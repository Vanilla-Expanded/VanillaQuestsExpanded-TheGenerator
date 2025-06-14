
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using System;

namespace VanillaQuestsExpandedTheGenerator
{
    [StaticConstructorOnStartup]
    public class CompRefuelableWithOverdrive : CompRefuelable
    {
        public Building_GenetronOverdrive building;
        public Building_GenetronNuclear building_nuclear;

        private CompFlickable flickComp;
        public float overdriveMultiplier = 1;
        public float tuningMultiplier = 1;

        public float constant1 = 0.0005f;
        public float constant2 = 0.03f;

        public int maxTuningMultiplierTimer = 0;
        new public CompProperties_RefuelableWithOverdrive Props => (CompProperties_RefuelableWithOverdrive)props;

        public float permanentFuelRodCalibrationMultiplier = 1;

        private static readonly Texture2D SetTargetUraniumLevelCommand = ContentFinder<Texture2D>.Get("UI/Gizmos/SetTargetUraniumLevel_Gizmo");

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref this.tuningMultiplier, "tuningMultiplier", 1, false);
            Scribe_Values.Look(ref this.maxTuningMultiplierTimer, "maxTuningMultiplierTimer", 0, false);
            Scribe_Values.Look(ref this.permanentFuelRodCalibrationMultiplier, "permanentFuelRodCalibrationMultiplier", 1, false);

        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            building = this.parent as Building_GenetronOverdrive;
            building_nuclear = this.parent as Building_GenetronNuclear;
            flickComp = parent.GetComp<CompFlickable>();
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            Genetron_MapComponent mapComp = this.parent.Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.AddRefuelableToMap(this.parent);
            }
        }
        public override void PostDeSpawn(Map map, DestroyMode mode)
        {
            Genetron_MapComponent mapComp = map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveRefuelableFromMap(this.parent);
            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {

            Genetron_MapComponent mapComp = previousMap.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveRefuelableFromMap(this.parent);
            }
        }

        public float ConsumptionRatePerTick
        {
            get {
                if (building.criticalBreakdown)
                {
                    return 0;
                }

                if (building.overdrive)
                {
                    overdriveMultiplier = 3;
                }
                else { overdriveMultiplier = 1; }
                if (!Props.isNuclear)
                {                  
                    return (Props.fuelConsumptionRate * overdriveMultiplier * tuningMultiplier) / 60000f;
                }
                else
                {
                    if(building_nuclear?.compPower.inFuelRodCalibrationMode == true)
                    {
                        return 0;
                    }
                    
                    return Math.Max(((constant1 * permanentFuelRodCalibrationMultiplier * Fuel * Fuel + constant2 * permanentFuelRodCalibrationMultiplier * Fuel) * overdriveMultiplier),1) / 60000f;
                    

                }
                
            }    
        }
        
        public override void CompTick()
        {
            if (building_nuclear?.permanentlyDisabled != true)
            {
                CompPowerTrader comp = parent.GetComp<CompPowerTrader>();
                if (!Props.consumeFuelOnlyWhenUsed && (flickComp == null || flickComp.SwitchIsOn) && (!Props.consumeFuelOnlyWhenPowered || (comp != null && comp.PowerOn)) && !Props.externalTicking)
                {
                    ConsumeFuel(this.ConsumptionRatePerTick);
                }
                if (Props.fuelConsumptionPerTickInRain > 0f && parent.Spawned && parent.Map.weatherManager.RainRate > 0.4f && !parent.Map.roofGrid.Roofed(parent.Position) && !Props.externalTicking)
                {
                    ConsumeFuel(Props.fuelConsumptionPerTickInRain);
                }
                if (tuningMultiplier == 2 && HasFuel)
                {
                    maxTuningMultiplierTimer++;
                }
                else { maxTuningMultiplierTimer = 0; }
                if (building_nuclear != null && Props.isNuclear)
                {
                    building_nuclear.Signal_ReduceMaintenanceBy(0.005f * this.ConsumptionRatePerTick);
                    if (Fuel <= 0.11)
                    {
                        ConsumeFuel(0.11f);
                        building_nuclear.permanentlyDisabled = true;
                    }
                }
            }
            

        }

        public override string CompInspectStringExtra()
        {
           
            string text = Props.FuelLabel + ": " + Fuel.ToStringDecimalIfSmall() + " / " + Props.fuelCapacity.ToStringDecimalIfSmall();
            if (!Props.consumeFuelOnlyWhenUsed && HasFuel)
            {
                int numTicks = 0;
                if (!Props.isNuclear)
                {
                    numTicks = (int)(Fuel / Props.fuelConsumptionRate * 60000f / (overdriveMultiplier * tuningMultiplier));
                    text = text + " (" + numTicks.ToStringTicksToPeriod() + ")";
                }
                else
                {
                    text = text + " (" + "VQE_FuelRateAtTheMoment".Translate(Math.Max(((constant1 * permanentFuelRodCalibrationMultiplier * Fuel * Fuel 
                        + constant2* permanentFuelRodCalibrationMultiplier * Fuel) * overdriveMultiplier),1).ToStringDecimalIfSmall()) + ")";
                }
                
                
                if (overdriveMultiplier != 1)
                {
                    text = text + " ("+"VQE_Overdrive".Translate()+")";
                }
            }
            if (Props.isNuclear && !HasFuel)
            {
                text = text + " (" + "VQE_ReactorShutdown".Translate() + ")";
            }
            
            if (Props.targetFuelLevelConfigurable)
            {
                text += "\n" + "ConfiguredTargetFuelLevel".Translate(TargetFuelLevel.ToStringDecimalIfSmall());
            }
            return text;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo c in base.CompGetGizmosExtra())
            {
                yield return c;
            }
            if (Props.uraniumLevelConfigurable)
            {
                Command_SetTargetUraniumLevel command_SetTargetFuelLevel = new Command_SetTargetUraniumLevel();
                command_SetTargetFuelLevel.refuelable = this;
                command_SetTargetFuelLevel.defaultLabel = "VQE_SetTargetUraniumLevel".Translate();
                command_SetTargetFuelLevel.defaultDesc = "VQE_SetTargetUraniumLevelDesc".Translate();
                command_SetTargetFuelLevel.icon = SetTargetUraniumLevelCommand;
                yield return command_SetTargetFuelLevel;
            }
           

        }


    }


}
