using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_WoodFired : Building_Genetron
    {

        public const int totalFuelBurnedToUpdate = 700; 

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();

            if (totalFuelBurned > totalFuelBurnedToUpdate)
            {
                command_Action.defaultDesc = "VQE_InstallWoodFueledGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_InstallWoodFueledGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_2", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_WoodFueled, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_InstallWoodFueledGenetronDescExpanded".Translate(totalFuelBurned);
                command_Action.defaultLabel = "VQE_InstallWoodFueledGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_2", true);
                command_Action.Disabled = true;                
            }           
            
            yield return command_Action;

        }






    }
}
