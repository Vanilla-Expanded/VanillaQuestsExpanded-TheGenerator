using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronWithFuelRodCalibration : Building_GenetronNuclear
    {
        public bool fuelRodCalibrationCanBeReUsed = true;
        public const int fuelRodCalibrationCanBeReUsedTime = 900000; // 15 days
        public int fuelRodCalibrationCanBeReUsedTimer = 0;
        public const int fuelRodCalibrationTime = 60000; // 1 day
        public int fuelRodCalibrationTimer = 0;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.fuelRodCalibrationCanBeReUsed, "fuelRodCalibrationCanBeReUsed", false, false);
            Scribe_Values.Look(ref this.fuelRodCalibrationCanBeReUsedTimer, "fuelRodCalibrationCanBeReUsedTimer", 0, false);
            Scribe_Values.Look(ref this.fuelRodCalibrationTimer, "fuelRodCalibrationTimer", 0, false);

        }

        public override void Tick()
        {
            base.Tick();
            if (!fuelRodCalibrationCanBeReUsed)
            {
                fuelRodCalibrationCanBeReUsedTimer++;
                if (fuelRodCalibrationCanBeReUsedTimer > fuelRodCalibrationCanBeReUsedTime)
                {
                    Signal_FuelRodCalibrationOffCooldown();
                }
                
            }
            if (compPower.inFuelRodCalibrationMode)
            {

                fuelRodCalibrationTimer++;
                if (fuelRodCalibrationTimer > fuelRodCalibrationTime)
                {
                    Signal_FuelRodCalibrationEnded();
                    Signal_PermanentFuelCoeficientsDecrease();
                }
                if (compRefuelable?.HasFuel == false || criticalBreakdown)
                {
                    Signal_FuelRodCalibrationEnded();
                  
                }            
            }
        }

        public void Signal_FuelRodCalibrationOffCooldown()
        {
            fuelRodCalibrationCanBeReUsed = true;
            fuelRodCalibrationCanBeReUsedTimer = 0;
        }

        public void Signal_FuelRodCalibrationStarted()
        {
            fuelRodCalibrationCanBeReUsed = false;
            compPower.inFuelRodCalibrationMode = true;
        }

        public void Signal_FuelRodCalibrationEnded()
        {
            compPower.inFuelRodCalibrationMode = false;
        }

        public void Signal_PermanentFuelCoeficientsDecrease()
        {
            compRefuelableWithOverdrive.permanentFuelRodCalibrationMultiplier *= 0.9f;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            
            if (fuelRodCalibrationCanBeReUsed)
            {
                command_Action.defaultDesc = "VQE_CalibrateFuelRodsDesc".Translate();
                command_Action.defaultLabel = "VQE_CalibrateFuelRods".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/CalibrateFuelRods_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Signal_FuelRodCalibrationStarted();
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_CalibrateFuelRodsDesc".Translate()+ "VQE_CalibrateFuelRodsDescExtended".Translate(fuelRodCalibrationCanBeReUsedTime.ToStringTicksToPeriod(),(fuelRodCalibrationCanBeReUsedTime - fuelRodCalibrationCanBeReUsedTimer).ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_CalibrateFuelRods".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/CalibrateFuelRods_Gizmo", true);
                command_Action.Disabled = true;
            }
            yield return command_Action;
            
        }

        public override string GetInspectString()
        {
            if (compPower.inFuelRodCalibrationMode)
            {
                return base.GetInspectString() + "\n" + "VQE_ShutDownForCalibrations".Translate((fuelRodCalibrationTime- fuelRodCalibrationTimer).ToStringTicksToPeriod());
            }
            return base.GetInspectString();
        }
    }
}
