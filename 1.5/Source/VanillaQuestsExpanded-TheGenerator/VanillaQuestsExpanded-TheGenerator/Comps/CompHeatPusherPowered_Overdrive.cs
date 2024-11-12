using RimWorld;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class CompHeatPusherPowered_Overdrive : CompHeatPusher
    {
        protected CompPowerTrader powerComp;

        protected CompRefuelableWithOverdrive refuelableComp;

        protected CompBreakdownable breakdownableComp;

        public Building_GenetronOverdrive building;

        public override bool ShouldPushHeatNow
        {
            get
            {
                if (!base.ShouldPushHeatNow || !FlickUtility.WantsToBeOn(parent) || (powerComp != null && !powerComp.PowerOn) || (refuelableComp != null && !refuelableComp.HasFuel) || (breakdownableComp != null && breakdownableComp.BrokenDown))
                {
                    return false;
                }
                return true;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (parent.IsHashIntervalTick(60) && ShouldPushHeatNow)
            {
                if(building?.overdrive == true) {
                    GenTemperature.PushHeat(parent.PositionHeld, parent.MapHeld, 24);
                }
                else { GenTemperature.PushHeat(parent.PositionHeld, parent.MapHeld, Props.heatPerSecond); }
                
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            powerComp = parent.GetComp<CompPowerTrader>();
            refuelableComp = parent.GetComp<CompRefuelableWithOverdrive>();
            breakdownableComp = parent.GetComp<CompBreakdownable>();
            building = parent as Building_GenetronOverdrive;
        }
    }
}