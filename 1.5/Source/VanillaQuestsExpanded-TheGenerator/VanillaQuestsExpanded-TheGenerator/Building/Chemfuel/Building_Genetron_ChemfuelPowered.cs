using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_ChemfuelPowered : Building_GenetronWithMaintenance
    {
        public const int overdriveFinished = 5;
     

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();

            if (completedOverdriveSuccessfully)
            {
                command_Action.defaultDesc = "VQE_InstallChemfuelBoostedGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_InstallChemfuelBoostedGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_6", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_ChemfuelBoosted, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_InstallChemfuelBoostedGenetronDesc".Translate()+"VQE_InstallChemfuelBoostedGenetronDescExpanded".Translate(overdriveFinished,overdriveTimer.ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_InstallChemfuelBoostedGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_6", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;

        }


        




    }
}
