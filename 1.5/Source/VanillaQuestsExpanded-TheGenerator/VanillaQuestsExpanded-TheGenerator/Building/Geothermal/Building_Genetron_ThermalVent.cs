﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_ThermalVent : Building_GenetronWithSteamBoost
    {

        public const int totalRunningTicksToUpdate = 600000; //10 days

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();

            if (totalRunningTicks > totalRunningTicksToUpdate)
            {
                command_Action.defaultDesc = "VQE_InstallHeatPoweredGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_InstallHeatPoweredGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_12", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_HeatPowered, Position, Map, Rotation, Faction.OfPlayer, null);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_InstallHeatPoweredGenetronDesc".Translate()+"VQE_InstallHeatPoweredGenetronDescExpanded".Translate(totalRunningTicksToUpdate.ToStringTicksToPeriod(),totalRunningTicks.ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_InstallHeatPoweredGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeGenetron_Gizmo_12", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;



        }







    }
}