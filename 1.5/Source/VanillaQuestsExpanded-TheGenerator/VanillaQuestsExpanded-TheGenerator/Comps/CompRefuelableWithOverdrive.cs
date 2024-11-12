
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
namespace VanillaQuestsExpandedTheGenerator
{
    [StaticConstructorOnStartup]
    public class CompRefuelableWithOverdrive : CompRefuelable
    {
        Building_GenetronOverdrive building;
       
        private CompFlickable flickComp;
        float multiplier = 1;
        new public CompProperties_RefuelableWithOverdrive Props => (CompProperties_RefuelableWithOverdrive)props;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            building = this.parent as Building_GenetronOverdrive;
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
        public override void PostDeSpawn(Map map)
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

        private float ConsumptionRatePerTick
        {
            get {
                
                if (building.overdrive)
                {
                    multiplier = 3;
                }
                else { multiplier = 1; }
                return (Props.fuelConsumptionRate * multiplier) / 60000f;
            }    
        }
        
        public override void CompTick()
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
        }

        public override string CompInspectStringExtra()
        {
            if (Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars)
            {
                return string.Empty;
            }
            string text = Props.FuelLabel + ": " + Fuel.ToStringDecimalIfSmall() + " / " + Props.fuelCapacity.ToStringDecimalIfSmall();
            if (!Props.consumeFuelOnlyWhenUsed && HasFuel)
            {
                int numTicks = (int)(Fuel / Props.fuelConsumptionRate * 60000f / multiplier);
                text = text + " (" + numTicks.ToStringTicksToPeriod() + ")";
                if (multiplier != 1)
                {
                    text = text + " ("+"VQE_Overdrive".Translate()+")";
                }
            }
            if (!HasFuel && !Props.outOfFuelMessage.NullOrEmpty())
            {
                string arg = (parent.def.building != null && parent.def.building.IsTurret) ? ("CannotShoot".Translate() + ": " + Props.outOfFuelMessage).Resolve() : Props.outOfFuelMessage;
                text += $"\n{arg} ({GetFuelCountToFullyRefuel()}x {Props.fuelFilter.AnyAllowedDef.label})";
            }
            if (Props.targetFuelLevelConfigurable)
            {
                text += "\n" + "ConfiguredTargetFuelLevel".Translate(TargetFuelLevel.ToStringDecimalIfSmall());
            }
            return text;
        }
    }
}
