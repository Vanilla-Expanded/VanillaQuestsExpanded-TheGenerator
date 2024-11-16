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
        public float cachedMaintenanceLoss = 0;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.maintenance, "maintenance", 1, false);
            Scribe_Values.Look(ref this.cachedMaintenanceLoss, "cachedMaintenanceLoss", 0, false);

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
            base.Destroy(mode);
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveMaintainableFromMap(this);
            }
        }

        public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
        {
            base.Kill(dinfo, exactCulprit);
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveMaintainableFromMap(this);
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(100))
            {
                maintenance -= cachedMaintenanceLoss / 600;
            }
        }

        public override string GetInspectString()
        {
            return base.GetInspectString() +"\n"+ "VQE_CurrentMaintenance".Translate(maintenance.ToStringPercent());
            
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
