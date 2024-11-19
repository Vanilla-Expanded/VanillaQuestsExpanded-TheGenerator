using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;

using Verse;
using Verse.Noise;

namespace VanillaQuestsExpandedTheGenerator
{
    public static class Utils
    {


        public static float CalculateMaintenancePowerImpact(float multiplier)
        {
            return 0.5025f * multiplier + 0.495f;

        }
    }
}