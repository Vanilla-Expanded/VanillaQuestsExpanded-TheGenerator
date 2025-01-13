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
            if (loc.ToIntVec3().ShouldSpawnMotesAt(map))
            {
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.Smoke, Rand.Range(1.5f, 2.5f) * size);
                dataStatic.rotationRate = Rand.Range(-30f, 30f);
                dataStatic.velocityAngle = Rand.Range(30, 40);
                dataStatic.velocitySpeed = Rand.Range(0.5f, 0.7f);
                dataStatic.instanceColor = Color.black;
                map.flecks.CreateFleck(dataStatic);
            }
        }

        public static void ThrowExtendedAirPuffUp(Vector3 loc, Map map, float size, float speedMultiplier)
        {
            if (loc.ToIntVec3().ShouldSpawnMotesAt(map))
            {
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + new Vector3(Rand.Range(-0.02f, 0.02f), 0f, Rand.Range(-0.02f, 0.02f)), map, FleckDefOf.AirPuff, Rand.Range(1.5f, 2.5f) * size);
                dataStatic.rotationRate = Rand.RangeInclusive(-240, 240);
                dataStatic.velocityAngle = Rand.Range(-45, 45);
                dataStatic.velocitySpeed = Rand.Range(1.2f, 1.5f) * speedMultiplier;
                map.flecks.CreateFleck(dataStatic);
            }
        }


        public static bool HasStudiedBasicHediffOrBackstory(Pawn pawn)
        {
            if(pawn?.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VQE_StudiedAncientGenetron) != null)
            {
                return true;
            }
            if (pawn.story.Adulthood!=null && StaticCollections.basicBackstories.Contains(pawn?.story.Adulthood))
            {
                return true;
            }
            if (pawn.story.Childhood != null && StaticCollections.basicBackstories.Contains(pawn?.story.Childhood))
            {
                return true;
            }
            return false;
        }

        public static bool HasStudiedGeothermalHediffOrBackstory(Pawn pawn)
        {
            if (pawn?.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VQE_StudiedAncientGeothermalGenetron) != null)
            {
                return true;
            }
            if (pawn.story.Adulthood != null && StaticCollections.advancedBackstories.Contains(pawn?.story.Adulthood))
            {
                return true;
            }
            if (pawn.story.Childhood != null && StaticCollections.advancedBackstories.Contains(pawn?.story.Childhood))
            {
                return true;
            }
            return false;
        }

        public static bool HasStudiedNuclearHediffOrBackstory(Pawn pawn)
        {
            if (pawn?.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VQE_StudiedAncientNuclearGenetron) != null)
            {
                return true;
            }
            if (pawn.story.Adulthood != null && StaticCollections.advancedBackstories.Contains(pawn?.story.Adulthood))
            {
                return true;
            }
            if (pawn.story.Childhood != null && StaticCollections.advancedBackstories.Contains(pawn?.story.Childhood))
            {
                return true;
            }
            return false;
        }

        public static bool HasAnyStudiedHediffOrBackstory(Pawn pawn)
        {
            foreach (HediffDef hediffDef in StaticCollections.studiedHediffs) {
                if (pawn?.health?.hediffSet?.GetFirstHediffOfDef(hediffDef) != null)
                {
                    return true;
                }
            }            
            if (pawn.story.Adulthood != null && StaticCollections.advancedBackstories.Contains(pawn?.story.Adulthood))
            {
                return true;
            }
            if (pawn.story.Childhood != null && StaticCollections.advancedBackstories.Contains(pawn?.story.Childhood))
            {
                return true;
            }
            return false;
        }
    }
}