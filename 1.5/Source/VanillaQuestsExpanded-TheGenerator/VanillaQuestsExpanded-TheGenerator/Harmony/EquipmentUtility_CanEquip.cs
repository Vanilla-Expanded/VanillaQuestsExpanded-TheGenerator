
using HarmonyLib;
using RimWorld;
using Verse;
using System;



namespace VanillaQuestsExpandedTheGenerator
{
    [HarmonyPatch(typeof(EquipmentUtility), "CanEquip", new Type[] { typeof(Thing), typeof(Pawn), typeof(string), typeof(bool) }, new ArgumentType[]
        {0,0,ArgumentType.Out,0})]
    internal class VanillaQuestsExpandedTheGenerator_EquipmentUtility_CanEquip_Postfix
    {


        [HarmonyPostfix]
        private static void PostFix(ref bool __result, Thing thing, Pawn pawn, ref string cantReason)
        {
            if (pawn.Ideo?.HasPrecept(InternalDefOf.VQE_Technology_Rejected) ==true && thing.def.techLevel > TechLevel.Medieval)
            {
                __result = false;
                cantReason = "VQE_NoTechnology".Translate();
            }


        }
    }
}