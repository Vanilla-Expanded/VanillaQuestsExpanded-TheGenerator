using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronWithCalibration : Building_GenetronWithPowerSurge
    {
        public bool calibrationCanBeReUsed = true;
        public const int calibrationCanBeReUsedTime = 300000; // 5 days
        public int calibrationCanBeReUsedTimer = 0;
        public const int calibrationTime = 60000; // 1 day
        public int calibrationTimer = 0;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.calibrationCanBeReUsed, "calibrationCanBeReUsed", false, false);
            Scribe_Values.Look(ref this.calibrationCanBeReUsedTimer, "calibrationCanBeReUsedTimer", 0, false);
            Scribe_Values.Look(ref this.calibrationTimer, "calibrationTimer", 0, false);

        }

        public override void Tick()
        {
            base.Tick();
            if (!calibrationCanBeReUsed)
            {
                calibrationCanBeReUsedTimer++;
                if (calibrationCanBeReUsedTimer > calibrationCanBeReUsedTime)
                {
                    Signal_CalibrationOffCooldown();
                }
                
            }
            if (compPower.inCalibrationMode)
            {

                calibrationTimer++;
                if (calibrationTimer > calibrationTime)
                {
                    Signal_CalibrationEnded();
                    Signal_PermanentPowerIncrease();
                }
                if (compRefuelable?.HasFuel == false || criticalBreakdown)
                {
                    Signal_CalibrationEnded();
                  
                }            
            }
        }

        public void Signal_CalibrationOffCooldown()
        {
            calibrationCanBeReUsed = true;
            calibrationCanBeReUsedTimer = 0;
        }

        public void Signal_CalibrationStarted()
        {
            calibrationCanBeReUsed = false;
            compPower.inCalibrationMode = true;
        }

        public void Signal_CalibrationEnded()
        {
            compPower.inCalibrationMode = false;
        }

        public void Signal_PermanentPowerIncrease()
        {
            compPower.calibrationCounter += 1;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            
            if (calibrationCanBeReUsed)
            {
                command_Action.defaultDesc = "VQE_CalibrateEfficiencyDesc".Translate();
                command_Action.defaultLabel = "VQE_CalibrateEfficiency".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/CalibrateFuelEfficiency_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Signal_CalibrationStarted();
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_CalibrateEfficiencyDesc".Translate()+"VQE_CalibrateEfficiencyDescExtended".Translate(calibrationCanBeReUsedTime.ToStringTicksToPeriod(),(calibrationCanBeReUsedTime - calibrationCanBeReUsedTimer).ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_CalibrateEfficiency".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/CalibrateFuelEfficiency_Gizmo", true);
                command_Action.Disabled = true;
            }
            yield return command_Action;
            
        }

        public override string GetInspectString()
        {
            if (compPower.inCalibrationMode)
            {
                return base.GetInspectString() + "\n" + "VQE_LowPowerMode".Translate((calibrationTime-calibrationTimer).ToStringTicksToPeriod());
            }
            return base.GetInspectString();
        }
    }
}
