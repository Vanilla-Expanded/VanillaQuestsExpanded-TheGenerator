using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronTuning : Building_GenetronOverdrive
    {

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();
            if (cachedDetailsExtension?.steamTuningControl == true)
            {
                command_Action.defaultDesc = "VQE_GenetronSteamTuningDesc".Translate();
                command_Action.defaultLabel = "VQE_GenetronSteamTuning".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/TunePressure_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    /*Window_FineTuning tuningWindow = new Window_FineTuning(this as Building_GenetronWithMaintenance);
                    Find.WindowStack.Add(tuningWindow);*/
                };
            }
            else
            if (cachedDetailsExtension?.fineTuningControl == true)
            {
                command_Action.defaultDesc = "VQE_GenetronFineTuningDesc".Translate();
                command_Action.defaultLabel = "VQE_GenetronFineTuning".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/FineTuneGenerator_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Window_FineTuning tuningWindow = new Window_FineTuning(this as Building_GenetronWithMaintenance);
                    Find.WindowStack.Add(tuningWindow);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_GenetronTuningDesc".Translate();
                command_Action.defaultLabel = "VQE_GenetronTuning".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/TuneGenerator_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Window_Tuning tuningWindow = new Window_Tuning(this);
                    Find.WindowStack.Add(tuningWindow);
                };
            }

            

            yield return command_Action;
            if (DebugSettings.ShowDevGizmos)
            {

                Command_Action command_Action2 = new Command_Action();
                command_Action2.defaultLabel = "Set time on 200% to 100 days";
                command_Action2.action = delegate
                {
                    compRefuelableWithOverdrive.maxTuningMultiplierTimer = 6000000;
                };
                yield return command_Action2;



            }


        }







    }
}
