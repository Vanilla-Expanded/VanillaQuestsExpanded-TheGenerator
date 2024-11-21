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
                command_Action.defaultDesc = "VQE_InstallWoodBlastingGenetronDesc".Translate()+"VQE_InstallWoodBlastingGenetronDescExpanded".Translate(totalRunningTicksToUpdate.ToStringTicksToPeriod(),totalRunningTicks.ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_InstallWoodBlastingGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_4", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;

        }






    }
}
