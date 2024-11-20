using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_ChemfuelFortified : Building_GenetronWithCalibration
    {



        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
          
            Command_Action command_Action = new Command_Action();
            if (!InternalDefOf.GeothermalPower.IsFinished)
            {
                command_Action.defaultDesc = "VQE_InstallGeothermalGenetronDescNoResearch".Translate();
                command_Action.defaultLabel = "VQE_InstallGeothermalGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_9", true);
                command_Action.Disabled = true;

            }
            else
            if (!Genetron_GameComponent.Instance.geothermalGenetronStudied)
            {
                command_Action.defaultDesc = "VQE_InstallGeothermalGenetronDescNoStudied".Translate();
                command_Action.defaultLabel = "VQE_InstallGeothermalGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_9", true);
              
                command_Action.Disabled = true;
            }
            else
            {
                command_Action.defaultDesc = "VQE_InstallGeothermalGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_InstallGeothermalGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_9", true);
                
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_Geothermal, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }

            yield return command_Action;


        }







    }
}
