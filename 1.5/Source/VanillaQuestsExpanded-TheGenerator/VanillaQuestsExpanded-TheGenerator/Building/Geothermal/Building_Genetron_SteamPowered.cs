using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_SteamPowered : Building_GenetronWithSteamBoost
    {

        public const int steamBoostUsesNeeded = 3;

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            if (steamBoostUsedCounter >= steamBoostUsesNeeded)
            {
                command_Action.defaultDesc = "VQE_InstallThermalVentGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_InstallThermalVentGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_11", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_ThermalVent, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_InstallThermalVentGenetronDesc".Translate()+"VQE_InstallThermalVentGenetronDescExpanded".Translate(steamBoostUsesNeeded,steamBoostUsedCounter);
                command_Action.defaultLabel = "VQE_InstallThermalVentGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_11", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;

            Command_Action command_Action2 = new Command_Action();


            command_Action2.defaultDesc = "VQE_DowngradeGenetronDesc".Translate();
            command_Action2.defaultLabel = "VQE_DowngradeGenetron".Translate(InternalDefOf.VQE_Genetron_Geothermal.LabelCap);
            command_Action2.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/DowngradeGenetron_Gizmo_9", true);
            command_Action2.hotKey = KeyBindingDefOf.Misc2;
            command_Action2.action = delegate
            {
                Window_Downgrade downgradeWindow = new Window_Downgrade(this, InternalDefOf.VQE_Genetron_Geothermal);
                Find.WindowStack.Add(downgradeWindow);
            };


            yield return command_Action2;

        }







    }
}
