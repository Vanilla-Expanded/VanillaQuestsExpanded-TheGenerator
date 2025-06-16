using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_Atomic : Building_GenetronWithEmergencyShutDown
    {



        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

           
                command_Action.defaultDesc = "VQE_DowngradeGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_DowngradeGenetron".Translate(InternalDefOf.VQE_Genetron_Isotopic.LabelCap);
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/DowngradeGenetron_Gizmo_15", true);
                command_Action.hotKey = KeyBindingDefOf.Misc2;
                command_Action.action = delegate
                {
                    Window_Downgrade downgradeWindow = new Window_Downgrade(this, InternalDefOf.VQE_Genetron_Isotopic);
                    Find.WindowStack.Add(downgradeWindow);
                };
           

            yield return command_Action;


        }







    }
}
