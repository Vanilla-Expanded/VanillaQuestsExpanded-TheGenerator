using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_WoodPowered : Building_GenetronOverdrive
    {


        public const int totalRunningTicksToUpdate = 1800000; //30 days

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();

            if (totalRunningTicks > totalRunningTicksToUpdate)
            {
                command_Action.defaultDesc = "VQE_InstallWoodBlastingGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_InstallWoodBlastingGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_4", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_WoodBlasting, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_InstallWoodBlastingGenetronDesc".Translate();
                command_Action.defaultDescPostfix = "VQE_InstallWoodBlastingGenetronDescExpanded".Translate(totalRunningTicksToUpdate.ToStringTicksToPeriod(), totalRunningTicks.ToStringTicksToPeriod()).Colorize(Utils.tooltipColour);

                command_Action.defaultLabel = "VQE_InstallWoodBlastingGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_4", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;

            Command_Action command_Action2 = new Command_Action();


            command_Action2.defaultDesc = "VQE_DowngradeGenetronDesc".Translate();
            command_Action2.defaultLabel = "VQE_DowngradeGenetron".Translate(InternalDefOf.VQE_Genetron_WoodFueled.LabelCap);
            command_Action2.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/DowngradeGenetron_Gizmo_2", true);
            command_Action2.hotKey = KeyBindingDefOf.Misc2;
            command_Action2.action = delegate
            {
                Window_Downgrade downgradeWindow = new Window_Downgrade(this, InternalDefOf.VQE_Genetron_WoodFueled);
                Find.WindowStack.Add(downgradeWindow);
            };


            yield return command_Action2;

        }






    }
}
