using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_Nuclear : Building_GenetronWithFuelRodCalibration
    {

        public const int totalFuelBurnedToUpdate = 500;


        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            if (nuclearConsumptionTotal > totalFuelBurnedToUpdate)
            {
                command_Action.defaultDesc = "VQE_InstallIsotopicGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_InstallIsotopicGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_15", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_Isotopic, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_InstallIsotopicGenetronDesc".Translate() + "VQE_InstallIsotopicGenetronDescExpanded".Translate(totalFuelBurnedToUpdate, nuclearConsumptionTotal);
                command_Action.defaultLabel = "VQE_InstallIsotopicGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_15", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;


        }







    }
}
