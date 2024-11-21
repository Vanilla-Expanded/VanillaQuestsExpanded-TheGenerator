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

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.permanentlyDisabled, "permanentlyDisabled", false, false);


        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                compRefuelable.Refuel(50);

            }
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.AddUraniumFueledToMap(this);
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
            if (permanentlyDisabled)
            {
                Command_Action command_Action = new Command_Action();



                if (compRefuelable.Fuel >= minUraniumToRestart)
                {
                    command_Action.defaultDesc = "VQE_RestartGenetronDesc".Translate(minUraniumToRestart);
                    command_Action.defaultLabel = "VQE_RestartGenetron".Translate();
                    command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/RestartGenetron_Gizmo", true);
                    command_Action.hotKey = KeyBindingDefOf.Misc1;
                    command_Action.action = delegate
                    {
                        permanentlyDisabled = false;
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
            }

            
        }
    }





}

