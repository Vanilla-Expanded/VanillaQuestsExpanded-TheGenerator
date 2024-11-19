using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_ChemfuelBoosted : Building_GenetronWithPowerSurge
    {



        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            if (powerSurgeUsedCounter>=3)
            {
                command_Action.defaultDesc = "VQE_InstallChemfuelChargedGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_InstallChemfuelChargedGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_7", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_ChemfuelCharged, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_InstallChemfuelChargedGenetronDescExpanded".Translate(powerSurgeUsedCounter);
                command_Action.defaultLabel = "VQE_InstallChemfuelChargedGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_7", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;



        }







    }
}
