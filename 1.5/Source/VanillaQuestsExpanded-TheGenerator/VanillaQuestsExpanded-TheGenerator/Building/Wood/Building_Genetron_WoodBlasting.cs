using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_WoodBlasting : Building_GenetronTuning
    {

        public const int totalTimeInFullTuning = 720000; //12 days

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();
            if (!InternalDefOf.BiofuelRefining.IsFinished)
            {
                command_Action.defaultDesc = "VQE_InstallChemfuelPoweredGenetronDesc".Translate();
                command_Action.defaultDescPostfix = "VQE_InstallChemfuelPoweredGenetronDescNoResearch".Translate().Colorize(Utils.tooltipColour);
                command_Action.defaultLabel = "VQE_InstallChemfuelPoweredGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_5", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.Disabled = true;

            }
            else
            if (compRefuelableWithOverdrive.maxTuningMultiplierTimer > totalTimeInFullTuning)
            {
                command_Action.defaultDesc = "VQE_InstallChemfuelPoweredGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_InstallChemfuelPoweredGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_5", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_ChemfuelPowered, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_InstallChemfuelPoweredGenetronDesc".Translate();
                command_Action.defaultDescPostfix = "VQE_InstallChemfuelPoweredGenetronDescExpanded".Translate(totalTimeInFullTuning.ToStringTicksToPeriod(), compRefuelableWithOverdrive.maxTuningMultiplierTimer.ToStringTicksToPeriod()).Colorize(Utils.tooltipColour);
                command_Action.defaultLabel = "VQE_InstallChemfuelPoweredGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_5", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;

            Command_Action command_Action2 = new Command_Action();


            command_Action2.defaultDesc = "VQE_DowngradeGenetronDesc".Translate();
            command_Action2.defaultLabel = "VQE_DowngradeGenetron".Translate(InternalDefOf.VQE_Genetron_WoodPowered.LabelCap);
            command_Action2.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/DowngradeGenetron_Gizmo_3", true);
            command_Action2.hotKey = KeyBindingDefOf.Misc2;
            command_Action2.action = delegate
            {
                Window_Downgrade downgradeWindow = new Window_Downgrade(this, InternalDefOf.VQE_Genetron_WoodPowered);
                Find.WindowStack.Add(downgradeWindow);
            };


            yield return command_Action2;

        }


        




    }
}
