using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronWithPowerSurge : Building_GenetronWithMaintenance
    {

        public bool powerSurgeCanBeReUsed = true;
        public int powerSurgeCanBeReUsedTime = 900000; // 15 days
        public int powerSurgeCanBeReUsedTimer = 0;
        public int powerSurgeUsedCounter = 0;
        public static List<CompPowerBattery> batteriesShuffled = new List<CompPowerBattery>();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.powerSurgeCanBeReUsed, "powerSurgeCanBeReUsed", false, false);
            Scribe_Values.Look(ref this.powerSurgeCanBeReUsedTimer, "powerSurgeCanBeReUsedTimer", 0, false);
            Scribe_Values.Look(ref this.powerSurgeUsedCounter, "powerSurgeUsedCounter", 0, false);

        }

        public override void Tick()
        {
            base.Tick();           
            if (!powerSurgeCanBeReUsed)
            {
                powerSurgeCanBeReUsedTimer++;
                if (powerSurgeCanBeReUsedTimer > powerSurgeCanBeReUsedTime)
                {
                    Signal_PowerSurgeOffCooldown();              
                }
            }
        }

        public void Signal_PowerSurgeOffCooldown()
        {
            powerSurgeCanBeReUsed = true;
            powerSurgeCanBeReUsedTimer = 0;
        }

        public void Signal_PowerSurge()
        {
            powerSurgeUsedCounter++;
            powerSurgeCanBeReUsed = false;
            float energyProduced = 35 * compRefuelableWithOverdrive.Fuel / 2;
            compRefuelableWithOverdrive.ConsumeFuel(compRefuelableWithOverdrive.Fuel / 2);
            maintenance -= 0.25f;
            if (compPower.PowerNet.batteryComps.Any())
            {
                batteriesShuffled.Clear();
                batteriesShuffled.AddRange(compPower.PowerNet.batteryComps);
                batteriesShuffled.Shuffle();
                int num = 0;
                do
                {
                    num++;
                    if (num > 10000)
                    {
                        Log.Error("Too many iterations.");
                        break;
                    }
                    float num2 = float.MaxValue;
                    for (int i = 0; i < batteriesShuffled.Count; i++)
                    {
                        num2 = Mathf.Min(num2, batteriesShuffled[i].AmountCanAccept);
                    }
                    if (energyProduced >= num2 * (float)batteriesShuffled.Count)
                    {
                        for (int num3 = batteriesShuffled.Count - 1; num3 >= 0; num3--)
                        {
                            float amountCanAccept = batteriesShuffled[num3].AmountCanAccept;
                            bool num4 = amountCanAccept <= 0f || amountCanAccept == num2;
                            if (num2 > 0f)
                            {
                                batteriesShuffled[num3].AddEnergy(num2);
                                energyProduced -= num2;
                            }
                            if (num4)
                            {
                                batteriesShuffled.RemoveAt(num3);
                            }
                        }
                        continue;
                    }
                    float amount = energyProduced / (float)batteriesShuffled.Count;
                    for (int j = 0; j < batteriesShuffled.Count; j++)
                    {
                        batteriesShuffled[j].AddEnergy(amount);
                    }
                   
                    break;
                }
                while (!(energyProduced < 0.0005f) && batteriesShuffled.Any());
                batteriesShuffled.Clear();
            }

           
            GenExplosion.DoExplosion(Position + IntVec3.North * 4, Map, 4.9f, DamageDefOf.EMP, this, -1, -1f);
            GenExplosion.DoExplosion(Position + IntVec3.South * 4, Map, 4.9f, DamageDefOf.EMP, this, -1, -1f);
            GenExplosion.DoExplosion(Position + IntVec3.West * 4, Map, 4.9f, DamageDefOf.EMP, this, -1, -1f);
            GenExplosion.DoExplosion(Position + IntVec3.East * 4, Map, 4.9f, DamageDefOf.EMP, this, -1, -1f);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            if (maintenance<0.3f)
            {
                command_Action.defaultDesc = "VQE_PowerSurgeDescNoMaintenance".Translate();
                command_Action.defaultLabel = "VQE_PowerSurge".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/PowerSurge_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.Disabled = true;
            }
            else
            if (powerSurgeCanBeReUsed)
            {
                command_Action.defaultDesc = "VQE_PowerSurgeDesc".Translate();
                command_Action.defaultLabel = "VQE_PowerSurge".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/PowerSurge_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Signal_PowerSurge();
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_PowerSurgeDescExtended".Translate((powerSurgeCanBeReUsedTime - powerSurgeCanBeReUsedTimer).ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_PowerSurge".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/PowerSurge_Gizmo", true);
                command_Action.Disabled = true;
            }
            yield return command_Action;
            if (DebugSettings.ShowDevGizmos)
            {
                Command_Action command_Action3 = new Command_Action();
                command_Action3.defaultLabel = "Reset power surge cooldown";
                command_Action3.action = delegate
                {
                    Signal_PowerSurgeOffCooldown();
                };
                yield return command_Action3;
                Command_Action command_Action4 = new Command_Action();
                command_Action4.defaultLabel = "Set power surge usage to 10";
                command_Action4.action = delegate
                {
                    powerSurgeUsedCounter = 10;
                };
                yield return command_Action4;
            }
        }
    }
}
