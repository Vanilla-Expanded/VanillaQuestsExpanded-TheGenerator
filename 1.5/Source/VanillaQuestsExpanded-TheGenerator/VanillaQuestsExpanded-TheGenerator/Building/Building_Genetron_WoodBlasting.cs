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
                command_Action.defaultDesc = "VQE_InstallChemfuelPoweredGenetronDescExpanded".Translate(compRefuelableWithOverdrive.maxTuningMultiplierTimer.ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_InstallChemfuelPoweredGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_5", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;

        }


        




    }
}
