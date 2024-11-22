using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_UraniumPowered : Building_GenetronNuclear
    {



        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();

            if (usedRestartAtLeastOnce)
            {
                command_Action.defaultDesc = "VQE_InstallNuclearGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_InstallNuclearGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_14", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_Nuclear, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_InstallNuclearGenetronDesc".Translate() + "VQE_InstallNuclearGenetronDescExpanded".Translate();
                command_Action.defaultLabel = "VQE_InstallNuclearGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_14", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;



        }







    }
}
