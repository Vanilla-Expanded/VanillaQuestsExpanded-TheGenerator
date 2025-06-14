using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronWithSteamBoost : Building_GenetronWithCalibration
    {
        public bool steamBoostCanBeReUsed = true;
        public const int steamBoostCanBeReUsedTime = 900000; // 15 days
        public int steamBoostCanBeReUsedTimer = 0;
        public const int steamBoostTime = 300000; // 5 day
        public int steamBoostTimer = 0;
        public int steamBoostUsedCounter = 0;


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.steamBoostCanBeReUsed, "steamBoostCanBeReUsed", false, false);
            Scribe_Values.Look(ref this.steamBoostCanBeReUsedTimer, "steamBoostCanBeReUsedTimer", 0, false);
            Scribe_Values.Look(ref this.steamBoostTimer, "steamBoostTimer", 0, false);
            Scribe_Values.Look(ref this.steamBoostUsedCounter, "steamBoostUsedCounter", 0, false);

        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            
        }

        protected override void Tick()
        {
            base.Tick();
            if (!steamBoostCanBeReUsed)
            {
                steamBoostCanBeReUsedTimer++;
                if (steamBoostCanBeReUsedTimer > steamBoostCanBeReUsedTime)
                {
                    Signal_SteamBoostOffCooldown();
                }
                
            }
            if (compPower.inSteamBoostMode)
            {
                overdriveSprayer.SteamSprayerTick();
                steamBoostTimer++;
                if (steamBoostTimer > steamBoostTime)
                {
                    Signal_SteamBoostEnded();
                   
                }
                if (compBreakdownable?.BrokenDown == true)
                {
                    Signal_SteamBoostEnded();
                    Signal_Explode();
                  
                }            
            }
        }

        public void Signal_SteamBoostOffCooldown()
        {
            steamBoostCanBeReUsed = true;
            steamBoostCanBeReUsedTimer = 0;
        }

        public void Signal_SteamBoostStarted()
        {
            steamBoostCanBeReUsed = false;
            compPower.inSteamBoostMode = true;
            steamBoostUsedCounter++;
        }

        public void Signal_SteamBoostEnded()
        {
            compPower.inSteamBoostMode = false;
        }

        

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            
            if (steamBoostCanBeReUsed)
            {
                command_Action.defaultDesc = "VQE_SteamBoostDesc".Translate();
                command_Action.defaultLabel = "VQE_SteamBoost".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/SteamBoost_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Signal_SteamBoostStarted();
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_SteamBoostDesc".Translate()+"VQE_SteamBoostDescExtended".Translate(steamBoostCanBeReUsedTime.ToStringTicksToPeriod(),(steamBoostCanBeReUsedTime - steamBoostCanBeReUsedTimer).ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_SteamBoost".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/SteamBoost_Gizmo", true);
                command_Action.Disabled = true;
            }
            yield return command_Action;
            if (DebugSettings.ShowDevGizmos)
            {
                Command_Action command_Action3 = new Command_Action();
                command_Action3.defaultLabel = "Reset steam boost cooldown";
                command_Action3.action = delegate
                {
                    Signal_SteamBoostOffCooldown();
                };
                yield return command_Action3;
                Command_Action command_Action4 = new Command_Action();
                command_Action4.defaultLabel = "Set steam boost usage to 10";
                command_Action4.action = delegate
                {
                    steamBoostUsedCounter = 10;
                };
                yield return command_Action4;
               
            }
        }

       
    }
}
