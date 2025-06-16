using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;




namespace VanillaQuestsExpandedTheGenerator
{


    [HarmonyPatch(typeof(PowerNet))]
    [HarmonyPatch("PowerNetTick")]
    public static class VanillaQuestsExpandedTheGenerator_PowerNet_PowerNetTick_Patch
    {
        public static bool PowerUpActive(Thing parent) => parent.Spawned && parent.Map.GameConditionManager.ElectricityDisabled(parent.Map);
        public static bool PowerUpActive(CompPower powerComp) => PowerUpActive(powerComp.parent) && powerComp.parent is Building_Genetron genetron && genetron.cachedDetailsExtension?.worksInSolarFlares==true;
        public static List<CompPowerBattery> batteriesShuffled = new List<CompPowerBattery>();
        public static IEnumerable<CompPowerTrader> PowerTradersSolarFlare(this PowerNet net) => net.powerComps.Where(x => x.parent is Building_Genetron genetron && genetron.cachedDetailsExtension.worksInSolarFlares);

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> PowerNetOnSolarFlareTranspiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var get_PowerOn = AccessTools.Method(typeof(CompPowerTrader), "get_PowerOn");
            var powerCompsField = AccessTools.Field(typeof(PowerNet), "powerComps");
            var found = false;
            var codes = codeInstructions.ToList();
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                yield return code;
                if (!found && i > 4 && code.opcode == OpCodes.Brfalse_S && codes[i - 1].Calls(get_PowerOn) && codes[i - 4].LoadsField(powerCompsField))
                {
                    found = true;
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, powerCompsField);
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 8);
                    yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<CompPowerTrader>), "get_Item"));
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(VanillaQuestsExpandedTheGenerator_PowerNet_PowerNetTick_Patch), nameof(VanillaQuestsExpandedTheGenerator_PowerNet_PowerNetTick_Patch.PowerUpActive), new[] { typeof(CompPower) }));
                    yield return new CodeInstruction(OpCodes.Brtrue_S, code.operand);
                }
            }
        }

        [HarmonyPostfix]
        static void PowerBatteries(PowerNet __instance)
        {
            if (__instance.Map.gameConditionManager.ElectricityDisabled(__instance.Map)&&__instance.batteryComps.Any())
            {
                foreach (CompPowerTrader t in __instance.PowerTradersSolarFlare())
                {
                    float energyProduced = t.EnergyOutputPerTick;
                    batteriesShuffled.Clear();
                    batteriesShuffled.AddRange(__instance.batteryComps);
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

               
            }


        }


    }








}
