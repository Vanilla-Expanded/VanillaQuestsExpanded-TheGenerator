
using RimWorld;
using Verse;
namespace VanillaQuestsExpandedTheGenerator
{
    public class CompPowerPlantGenetron : CompPowerPlant
    {
        Building_Genetron building;
        new public CompProperties_PowerGenetron Props => (CompProperties_PowerGenetron)props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            building = this.parent as Building_Genetron;
        }

        public override void CompTick()
        {
            base.CompTick();         
        }

        public override void UpdateDesiredPowerOutput()
        {
            if ((breakdownableComp != null && breakdownableComp.BrokenDown) || (flickableComp != null && !flickableComp.SwitchIsOn) || (autoPoweredComp != null && !autoPoweredComp.WantsToBeOn) || (toxifier != null && !toxifier.CanPolluteNow) || !base.PowerOn)
            {
                base.PowerOutput = 0f;
            }else if(refuelableComp != null && !refuelableComp.HasFuel)
            {
                base.PowerOutput = Props.powerWithoutFuel;
            }
            else
            {
                float multiplier = building.overdrive ? 2 : 1;
                base.PowerOutput = DesiredPowerOutput * multiplier;
            }
        }


    }
}