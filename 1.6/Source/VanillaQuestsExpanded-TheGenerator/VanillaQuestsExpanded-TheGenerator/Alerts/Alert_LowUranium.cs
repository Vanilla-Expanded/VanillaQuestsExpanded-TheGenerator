using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Alert_LowUranium : Alert
    {
        public Alert_LowUranium()
        {
            defaultPriority = AlertPriority.Medium;
            defaultLabel = "VQE_Alert_LowUranium".Translate();
            defaultExplanation = "VQE_Alert_LowUranium_Desc".Translate();
        }

        protected override Color BGColor
        {
            get
            {
                float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
                return new Color(num, num, num) * Color.yellow;
            }
        }

        public override AlertReport GetReport()
        {
           
            var map = Find.CurrentMap;
            if (map == null)
            {
                return AlertReport.Inactive;
            }

            return AlertReport.CulpritsAre(GetLowUranium(map).ToList());
        }

        public static IEnumerable<Thing> GetLowUranium(Map map)
        {
            var genetrons = map.GetComponent<Genetron_MapComponent>().uraniumFueled_InMap;

            foreach (var genetron in genetrons)
            {
                float fuel = ((Building_Genetron)genetron).compRefuelable.Fuel;
                if (fuel <5)
                {
                    yield return genetron;
                }                
                               
            }
        }
    }
}
