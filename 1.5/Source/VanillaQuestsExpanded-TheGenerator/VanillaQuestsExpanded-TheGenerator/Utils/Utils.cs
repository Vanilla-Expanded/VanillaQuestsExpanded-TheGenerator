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

        public static Color tooltipColour = Color.yellow;


        public static float CalculateMaintenancePowerImpact(float multiplier)
        {
            return 0.5025f * multiplier + 0.495f;

        }

        public static void ThrowBlackSmoke(Vector3 loc, Map map, float size)
        {
            if (loc.ShouldSpawnMotesAt(map))
            {
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.Smoke, Rand.Range(1.5f, 2.5f) * size);
                dataStatic.rotationRate = Rand.Range(-30f, 30f);
                dataStatic.velocityAngle = Rand.Range(30, 40);
                dataStatic.velocitySpeed = Rand.Range(0.5f, 0.7f);
                dataStatic.instanceColor = Color.black;
                map.flecks.CreateFleck(dataStatic);
            }
        }
    }
}