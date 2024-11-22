using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_Isotopic : Building_GenetronWithHazardModes
    {

        public const int totalTicksInHazardMode = 3600000; // 60 days

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            if (hazardModeCounter > totalTicksInHazardMode)
            {
                command_Action.defaultDesc = "VQE_InstallAtomicGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_InstallAtomicGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_16", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_Atomic, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_InstallAtomicGenetronDesc".Translate() + "VQE_InstallAtomicGenetronDescExpanded".Translate(totalTicksInHazardMode.ToStringTicksToPeriod(), hazardModeCounter.ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_InstallAtomicGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_16", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;


        }







    }
}
