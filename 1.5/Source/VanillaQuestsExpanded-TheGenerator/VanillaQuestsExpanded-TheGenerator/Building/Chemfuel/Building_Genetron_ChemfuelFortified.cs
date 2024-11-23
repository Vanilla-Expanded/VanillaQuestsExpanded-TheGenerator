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
                command_Action.defaultDesc = "VQE_InstallGeothermalGenetronDesc".Translate()+"VQE_InstallGeothermalGenetronDescNoResearch".Translate();
                command_Action.defaultLabel = "VQE_InstallGeothermalGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_9", true);
                command_Action.Disabled = true;

            }
            else
            if (!Genetron_GameComponent.Instance.geothermalGenetronStudied)
            {
                command_Action.defaultDesc = "VQE_InstallGeothermalGenetronDesc".Translate()+"VQE_InstallGeothermalGenetronDescNoStudied".Translate();
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



            Command_Action command_Action2 = new Command_Action();
            if (!InternalDefOf.ShipReactor.IsFinished)
            {
                command_Action2.defaultDesc = "VQE_InstallUraniumPoweredGenetronDesc".Translate() + "VQE_InstallUraniumPoweredGenetronDescNoResearch".Translate();
                command_Action2.defaultLabel = "VQE_InstallUraniumPoweredGenetron".Translate();
                command_Action2.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_13", true);
                command_Action2.Disabled = true;

            }
            else
            if (!Genetron_GameComponent.Instance.nuclearGenetronStudied)
            {
                command_Action2.defaultDesc = "VQE_InstallUraniumPoweredGenetronDesc".Translate() + "VQE_InstallUraniumPoweredGenetronDescNoStudied".Translate();
                command_Action2.defaultLabel = "VQE_InstallUraniumPoweredGenetron".Translate();
                command_Action2.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_13", true);

                command_Action2.Disabled = true;
            }
            else
            {
                command_Action2.defaultDesc = "VQE_InstallUraniumPoweredGenetronDesc".Translate();
                command_Action2.defaultLabel = "VQE_InstallUraniumPoweredGenetron".Translate();
                command_Action2.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_13", true);

                command_Action2.hotKey = KeyBindingDefOf.Misc2;
                command_Action2.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_UraniumPowered, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }

            yield return command_Action2;

            Command_Action command_Action3 = new Command_Action();


            command_Action3.defaultDesc = "VQE_DowngradeGenetronDesc".Translate();
            command_Action3.defaultLabel = "VQE_DowngradeGenetron".Translate(InternalDefOf.VQE_Genetron_ChemfuelFortified.LabelCap);
            command_Action3.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/DowngradeGenetron_Gizmo_8", true);
            command_Action3.hotKey = KeyBindingDefOf.Misc2;
            command_Action3.action = delegate
            {
                Window_Downgrade downgradeWindow = new Window_Downgrade(this, InternalDefOf.VQE_Genetron_ChemfuelFortified);
                Find.WindowStack.Add(downgradeWindow);
            };


            yield return command_Action3;


        }

    }
}
