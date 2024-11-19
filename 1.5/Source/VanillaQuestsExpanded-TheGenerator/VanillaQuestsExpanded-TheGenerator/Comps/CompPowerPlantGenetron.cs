
using RimWorld;
using Verse;
namespace VanillaQuestsExpandedTheGenerator
{
    public class CompPowerPlantGenetron : CompPowerPlant
    {
        public Building_GenetronOverdrive building;
        public Building_GenetronWithMaintenance building_withMaintenance;
        public bool inCalibrationMode = false;
        public int calibrationCounter = 0;

        new public CompProperties_PowerGenetron Props => (CompProperties_PowerGenetron)props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            building = this.parent as Building_GenetronOverdrive;
            building_withMaintenance = this.parent as Building_GenetronWithMaintenance;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref this.inCalibrationMode, "inCalibrationMode", false, false);
            Scribe_Values.Look(ref this.calibrationCounter, "calibrationCounter", 0, false);

        }

        public override void CompTick()
        {
            base.CompTick();         
        }

        public override void UpdateDesiredPowerOutput()
        {
            if ((building != null && building.criticalBreakdown) || (breakdownableComp != null && breakdownableComp.BrokenDown) || (flickableComp != null && !flickableComp.SwitchIsOn) || (autoPoweredComp != null && !autoPoweredComp.WantsToBeOn) || (toxifier != null && !toxifier.CanPolluteNow) || !base.PowerOn)
            {
                base.PowerOutput = 0f;
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

                base.PowerOutput = baseOutput * overdriveMultiplier * tuningMultiplier * maintenanceMultiplier * calibrationMultiplier * (1+(calibrationCounter*0.01f));
            }
        }


    }
}