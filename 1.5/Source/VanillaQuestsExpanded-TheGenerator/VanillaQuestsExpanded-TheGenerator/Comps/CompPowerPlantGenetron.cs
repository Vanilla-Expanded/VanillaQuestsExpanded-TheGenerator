
using RimWorld;
using Verse;
using Verse.Sound;
namespace VanillaQuestsExpandedTheGenerator
{
    public class CompPowerPlantGenetron : CompPowerPlant
    {
        public Building_GenetronOverdrive building;
        public Building_GenetronWithMaintenance building_withMaintenance;
        public Building_GenetronNuclear building_nuclear;
        public bool inCalibrationMode = false;
        public bool inFuelRodCalibrationMode = false;
        public bool inSteamBoostMode = false;
        public int calibrationCounter = 0;
      

        new public CompProperties_PowerGenetron Props => (CompProperties_PowerGenetron)props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            building = this.parent as Building_GenetronOverdrive;
            building_withMaintenance = this.parent as Building_GenetronWithMaintenance;
            building_nuclear = this.parent as Building_GenetronNuclear;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref this.inCalibrationMode, "inCalibrationMode", false, false);
            Scribe_Values.Look(ref this.inFuelRodCalibrationMode, "inFuelRodCalibrationMode", false, false);
            Scribe_Values.Look(ref this.inSteamBoostMode, "inSteamBoostMode", false, false);
            Scribe_Values.Look(ref this.calibrationCounter, "calibrationCounter", 0, false);
            

        }

        public override void CompTick()
        {
            base.CompTick();         
        }

        

        public override void UpdateDesiredPowerOutput()
        {
            if (inFuelRodCalibrationMode||(building != null && building.criticalBreakdown) || (breakdownableComp != null && breakdownableComp.BrokenDown) || 
                (flickableComp != null && !flickableComp.SwitchIsOn) || (autoPoweredComp != null && !autoPoweredComp.WantsToBeOn) ||
                 (building_nuclear?.permanentlyDisabled == true) ||
              (toxifier != null && !toxifier.CanPolluteNow) || !base.PowerOn)
            {
                base.PowerOutput = 0f;
            }else if (Props.isNuclear)
            {
                             
                    //Overdrive multiplier
                    float overdriveMultiplier = 1;
                    if (building?.overdrive == true)
                    {
                        overdriveMultiplier = 2;
                    }
                    float fuelAmount = building.compRefuelableWithOverdrive.Fuel;
                    base.PowerOutput = (5 * fuelAmount * fuelAmount + 50 * fuelAmount) * overdriveMultiplier;
                                            
            }
            else 
            {
                float baseOutput = DesiredPowerOutput;
                if (refuelableComp != null && !refuelableComp.HasFuel)
                {
                    baseOutput = Props.powerWithoutFuel;
                }

                //Overdrive multiplier
                float overdriveMultiplier = 1;
                if(building?.overdrive == true)
                {
                    overdriveMultiplier = 2;
                }

                //Tuning multiplier
                float tuningMultiplier = 1;
                if (building?.compRefuelableWithOverdrive != null)
                {
                    tuningMultiplier = building.compRefuelableWithOverdrive.tuningMultiplier;
                }

                //Maintenance multiplier
                float maintenanceMultiplier = 1;
                if (building_withMaintenance?.maintenanceMultiplier != null)
                {
                    maintenanceMultiplier = Utils.CalculateMaintenancePowerImpact(building_withMaintenance.maintenanceMultiplier);
                }

                //Calibration multiplier
                float calibrationMultiplier = 1;
                if (inCalibrationMode)
                {
                    calibrationMultiplier = 0.1f;
                }

                //Steam boost multiplier
                float steamBoostMultiplier = 1;
                if (inSteamBoostMode)
                {
                    steamBoostMultiplier = 1.2f;
                }

                base.PowerOutput = baseOutput * overdriveMultiplier * tuningMultiplier * maintenanceMultiplier * calibrationMultiplier * (1+(calibrationCounter*0.01f)) * steamBoostMultiplier;
            }
        }


    }
}