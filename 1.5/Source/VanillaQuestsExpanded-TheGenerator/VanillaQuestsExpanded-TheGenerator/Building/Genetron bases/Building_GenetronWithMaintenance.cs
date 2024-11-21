using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronWithMaintenance : Building_GenetronTuning
    {

        public float maintenance = 1;
        public float maintenanceMultiplier = 1;
        public float cachedMaintenanceLoss = 0;
        public float componentCalibrationMultiplier = 1;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.maintenance, "maintenance", 1, false);
            Scribe_Values.Look(ref this.maintenanceMultiplier, "maintenanceMultiplier", 1, false);
            Scribe_Values.Look(ref this.cachedMaintenanceLoss, "cachedMaintenanceLoss", 0, false);
            Scribe_Values.Look(ref this.componentCalibrationMultiplier, "componentCalibrationMultiplier", 1, false);

        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            cachedMaintenanceLoss = this.GetStatValue(InternalDefOf.VQE_GenetronMaintenanceLoss);
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.AddMaintainableToMap(this);
            }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveMaintainableFromMap(this);
            }
            base.Destroy(mode);
            
        }

        public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
        {
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveMaintainableFromMap(this);
            }
            base.Kill(dinfo, exactCulprit);
            
        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(100) && maintenance > 0 && cachedDetailsExtension?.nonLinearMaintenanceLoss != true)
            {
                maintenance -= (cachedMaintenanceLoss / 600)* maintenanceMultiplier * componentCalibrationMultiplier;                
            }
            if (maintenance <= 0)
            {
                maintenance = 0.1f;
                Signal_ChooseBreakdown();
            }
        }

        public void Signal_ReduceMaintenanceBy(float amount)
        {
            maintenance -= amount;
        }

        public void Signal_ChooseBreakdown()
        {          
            if (compBreakdownable != null && cachedDetailsExtension.noCriticalBreakdowns)
            {
                compBreakdownable.DoBreakdown();
            }
            if (cachedDetailsExtension?.hasNuclearMeltdowns==true)
            {
                Signal_NuclearCriticalBreakdown();

            }
            else { Signal_CriticalBreakdown(); }

        }

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
           
            if (DebugSettings.ShowDevGizmos)
            {

                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "Set maintenance to 10%";
                command_Action.action = delegate
                {
                    maintenance = 0.1f;
                };
                yield return command_Action;



            }


        }





    }
}
