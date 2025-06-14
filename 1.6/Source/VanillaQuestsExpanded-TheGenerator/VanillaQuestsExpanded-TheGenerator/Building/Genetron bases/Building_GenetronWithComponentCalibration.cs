using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronWithComponentCalibration : Building_GenetronWithSteamBoost
    {
        public bool calibrateComponentsCanBeReUsed = true;
        public const int calibrateComponentsCanBeReUsedTime = 180000; // 3 days
        public int calibrateComponentsCanBeReUsedTimer = 0;
        public bool inBreakdown = false;
      
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.calibrateComponentsCanBeReUsed, "calibrateComponentsCanBeReUsed", false, false);
            Scribe_Values.Look(ref this.calibrateComponentsCanBeReUsedTimer, "calibrateComponentsCanBeReUsedTimer", 0, false);
            Scribe_Values.Look(ref this.inBreakdown, "inBreakdown", false, false);

        }

        protected override void Tick()
        {
            base.Tick();
            if (!calibrateComponentsCanBeReUsed)
            {
                calibrateComponentsCanBeReUsedTimer++;
                if (calibrateComponentsCanBeReUsedTimer > calibrateComponentsCanBeReUsedTime)
                {
                    Signal_CalibrateComponentsOffCooldown();
                }      
            }
        }

        public void Signal_CalibrateComponentsOffCooldown()
        {
            calibrateComponentsCanBeReUsed = true;
            calibrateComponentsCanBeReUsedTimer = 0;
        }

        public void Signal_CalibrateComponentsStarted()
        {
            calibrateComponentsCanBeReUsed = false;
            Signal_ChooseBreakdown();
            inBreakdown = true;
        }

        public void Signal_Repaired()
        {           
            inBreakdown = false;
            componentCalibrationMultiplier *= 0.9f;
        }


        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();


            if (calibrateComponentsCanBeReUsed)
            {
                command_Action.defaultDesc = "VQE_CalibrateComponentsDesc".Translate();
                command_Action.defaultLabel = "VQE_CalibrateComponents".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/CalibrateComponents_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Signal_CalibrateComponentsStarted();
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_CalibrateComponentsDesc".Translate()+"VQE_CalibrateComponentsDescExtended".Translate(calibrateComponentsCanBeReUsedTime.ToStringTicksToPeriod(), (calibrateComponentsCanBeReUsedTime - calibrateComponentsCanBeReUsedTimer).ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_CalibrateComponents".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/CalibrateComponents_Gizmo", true);
                command_Action.Disabled = true;
            }
            yield return command_Action;

        }


    }
}
