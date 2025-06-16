using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronWithEmergencyShutDown : Building_GenetronWithHazardModes
    {
      

        public override void ExposeData()
        {
            base.ExposeData();
           

        }
    

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();


            if (!permanentlyDisabled)
            {
                command_Action.defaultDesc = "VQE_EmergencyShutDownDesc".Translate();
                command_Action.defaultLabel = "VQE_EmergencyShutDown".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/EmergencyShutDown_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    permanentlyDisabled = true;
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_EmergencyShutDownDesc".Translate() + "VQE_EmergencyShutDownDescExtended".Translate(fuelRodCalibrationCanBeReUsedTime.ToStringTicksToPeriod(), (fuelRodCalibrationCanBeReUsedTime - fuelRodCalibrationCanBeReUsedTimer).ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_EmergencyShutDown".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/EmergencyShutDown_Gizmo", true);
                command_Action.Disabled = true;
            }
            yield return command_Action;


        }

        
    }
}
