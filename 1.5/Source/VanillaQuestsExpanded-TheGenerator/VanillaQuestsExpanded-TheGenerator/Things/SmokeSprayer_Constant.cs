
using RimWorld;
using System;
using Verse;
using UnityEngine;
namespace VanillaQuestsExpandedTheGenerator
{
    public class SmokeSprayer_Constant
    {
        private Thing parent;

        private int ticksUntilSpray = 0;

        private int sprayTicksLeft;

        public Action startSprayCallback;

        public Action endSprayCallback;

        private const int MinTicksBetweenSprays = 50;

        private const int MaxTicksBetweenSprays = 50;

        private const int MinSprayDuration = 300;

        private const int MaxSprayDuration = 400;

        private const float SprayThickness = 0.4f;

        public SmokeSprayer_Constant(Thing parent)
        {
            this.parent = parent;
        }

        public void SteamSprayerTick()
        {
            if (sprayTicksLeft > 0)
            {
                sprayTicksLeft--;
                if (Rand.Value < SprayThickness)
                {
                    Utils.ThrowBlackSmoke(parent.TrueCenter()+ new Vector3(new FloatRange(0,5).RandomInRange, new FloatRange(0, 5).RandomInRange, new FloatRange(0, 5).RandomInRange), parent.Map, new FloatRange(0, 3).RandomInRange);
                }
                
                if (sprayTicksLeft <= 0)
                {
                    if (endSprayCallback != null)
                    {
                        endSprayCallback();
                    }
                    ticksUntilSpray = Rand.RangeInclusive(MinTicksBetweenSprays, MaxTicksBetweenSprays);
                }
                return;
            }
            ticksUntilSpray--;
            if (ticksUntilSpray <= 0)
            {
                if (startSprayCallback != null)
                {
                    startSprayCallback();
                }
                sprayTicksLeft = Rand.RangeInclusive(MinSprayDuration, MaxSprayDuration);
            }
        }
    }
}