using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_WoodPowered : Building_Genetron
    {

     

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();

            if (overdriveCanBeReUsed)
            {
                command_Action.defaultDesc = "VQE_GenetronOverdriveDesc".Translate();
                command_Action.defaultLabel = "VQE_GenetronOverdrive".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/GeneratorOverdrive_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;             
                command_Action.action = delegate
                {
                    overdrive = true;
                    overdriveCanBeReUsed = false;
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_GenetronOverdriveDescExtended".Translate((overdriveTime - overdriveTimer).ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_GenetronOverdrive".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/GeneratorOverdrive_Gizmo", true);
                command_Action.Disabled = true;
            }

            

            yield return command_Action;

        }


        




    }
}
