
using RimWorld;
using System;
using Verse;
using UnityEngine;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Genetron_SteamSprayer
    {
        private Building_Genetron parent;

        private int ticksUntilSpray = 250;

        private int sprayTicksLeft;

        public Action startSprayCallback;

        public Action endSprayCallback;

        private const int MinTicksBetweenSprays = 50;

        private const int MaxTicksBetweenSprays = 50;

        private const int MinSprayDuration = 500;

        private const int MaxSprayDuration = 500;

        private const float SprayThickness = 0.6f;

        public float RealThickness()
        {
            CompPowerPlantGenetron comp = parent.compPower;
            return (comp.powerOutputInt / (-comp.Props.PowerConsumption + comp.Props.powerWithoutFuel))* SprayThickness;

        }

        public Genetron_SteamSprayer(Building_Genetron parent)
        {
            this.parent = parent;
        }

        public void SteamSprayerTick()
        {
            if (sprayTicksLeft > 0)
            {
                sprayTicksLeft--;
                if (Rand.Value < RealThickness())
                {
                    FleckMaker.ThrowAirPuffUp(parent.TrueCenter()+new Vector3(0,0,2.75f), parent.Map);
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