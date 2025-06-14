using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronNuclear : Building_GenetronWithMaintenance
    {
        public bool permanentlyDisabled = false;
        public const int minUraniumToRestart = 50;
        public bool usedRestartAtLeastOnce = false;
     
        public float nuclearConsumptionTotal = 0;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.permanentlyDisabled, "permanentlyDisabled", false, false);
            Scribe_Values.Look(ref this.usedRestartAtLeastOnce, "usedRestartAtLeastOnce", false, false);
            Scribe_Values.Look(ref this.nuclearConsumptionTotal, "nuclearConsumptionTotal", 0, false);


        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad && !compRefuelable.HasFuel)
            {
                compRefuelable.Refuel(50);

            }
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.AddUraniumFueledToMap(this);
            }

            
        }

        protected override void Tick()
        {
            base.Tick();
            if (compRefuelableWithOverdrive?.HasFuel == true)
            {
                nuclearConsumptionTotal += compRefuelableWithOverdrive.ConsumptionRatePerTick;
            }
         

        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveUraniumFueledFromMap(this);
            }
            base.Destroy(mode);

        }

        public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
        {
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveUraniumFueledFromMap(this);
            }
            base.Kill(dinfo, exactCulprit);

        }

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();
            if (!permanentlyDisabled)
            {
                command_Action.defaultDesc = "VQE_RestartGenetronDesc".Translate(minUraniumToRestart) + "VQE_RestartGenetronDescNotNeeded".Translate();
                command_Action.defaultLabel = "VQE_RestartGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/RestartGenetron_Gizmo", true);
                command_Action.Disabled = true;

            }
            else

            if (compRefuelable.Fuel >= minUraniumToRestart)
            {
                command_Action.defaultDesc = "VQE_RestartGenetronDesc".Translate(minUraniumToRestart);
                command_Action.defaultLabel = "VQE_RestartGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/RestartGenetron_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    permanentlyDisabled = false;
                    usedRestartAtLeastOnce = true;
                };

            }
            else
            {
                command_Action.defaultDesc = "VQE_RestartGenetronDesc".Translate(minUraniumToRestart) + "VQE_RestartGenetronDescExtended".Translate();
                command_Action.defaultLabel = "VQE_RestartGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/RestartGenetron_Gizmo", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;

            if (DebugSettings.ShowDevGizmos)
            {
                Command_Action command_Action3 = new Command_Action();
                command_Action3.defaultLabel = "Fake a restart";
                command_Action3.action = delegate
                {
                    permanentlyDisabled = false;
                    usedRestartAtLeastOnce = true;
                };
                yield return command_Action3;

                Command_Action command_Action4 = new Command_Action();
                command_Action4.defaultLabel = "Set uranium used to 10000";
                command_Action4.action = delegate
                {
                    nuclearConsumptionTotal = 10000;
                };
                yield return command_Action4;
            }



        }
    }





}

