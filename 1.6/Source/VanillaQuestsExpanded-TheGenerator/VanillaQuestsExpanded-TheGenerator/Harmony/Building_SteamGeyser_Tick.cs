using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;




namespace VanillaQuestsExpandedTheGenerator
{


    [HarmonyPatch(typeof(Building_SteamGeyser))]
    [HarmonyPatch("Tick")]
    public static class VanillaQuestsExpandedTheGenerator_Building_SteamGeyser_Tick_Patch
    {



        [HarmonyPrefix]
        static bool DisableSteam(Building_SteamGeyser __instance)
        {
            if(Genetron_GameComponent.Instance.supressedGeysers.Contains(__instance))
            {
                return false;
            }
            return true;



        }
    }








}
