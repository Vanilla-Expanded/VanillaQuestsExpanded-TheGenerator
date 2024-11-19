
using RimWorld;
using Verse;
namespace VanillaQuestsExpandedTheGenerator
{
    public class CompPowerPlantGenetron : CompPowerPlant
    {
        public Building_GenetronOverdrive building;
        public Building_GenetronWithMaintenance building_withMaintenance;

        new public CompProperties_PowerGenetron Props => (CompProperties_PowerGenetron)props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            building = this.parent as Building_GenetronOverdrive;
            building_withMaintenance = this.parent as Building_GenetronWithMaintenance;
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
            }else if(refuelableComp != null && !refuelableComp.HasFuel)
            {
                base.PowerOutput = Props.powerWithoutFuel;
            }
            else
            {
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

                base.PowerOutput = DesiredPowerOutput * overdriveMultiplier * tuningMultiplier * maintenanceMultiplier;
            }
        }


    }
}