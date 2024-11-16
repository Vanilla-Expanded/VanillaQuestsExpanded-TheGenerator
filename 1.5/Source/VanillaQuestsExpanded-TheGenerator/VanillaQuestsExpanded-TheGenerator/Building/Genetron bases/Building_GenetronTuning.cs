using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronTuning  : Building_GenetronOverdrive
    {

        public float tuningMultiplier =1;
       
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.tuningMultiplier, "tuningMultiplier", 1, false);

        }       

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();

           
                command_Action.defaultDesc = "VQE_GenetronTuningDesc".Translate();
                command_Action.defaultLabel = "VQE_GenetronTuning".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/TuneGenerator_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Window_Tuning tuningWindow = new Window_Tuning(this);
                    Find.WindowStack.Add(tuningWindow);
                };
            
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
