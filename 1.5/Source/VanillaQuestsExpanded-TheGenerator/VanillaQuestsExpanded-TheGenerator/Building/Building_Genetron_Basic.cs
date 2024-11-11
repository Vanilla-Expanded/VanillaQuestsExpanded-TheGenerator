using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_Basic : Building_Genetron
    {

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            command_Action.defaultDesc = "VQE_InstallWoodFiredGenetronDesc".Translate();
            command_Action.defaultLabel = "VQE_InstallWoodFiredGenetron".Translate();
            command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/RestartGenetron_Gizmo", true);
            command_Action.hotKey = KeyBindingDefOf.Misc1;
            command_Action.action = delegate
            {
                GenConstruct.PlaceBlueprintForBuild(InternalDefOf.VQE_Genetron_WoodFired, Position, Map, Rotation, Faction.OfPlayer, null);


            };
            yield return command_Action;

        }






    }
}
