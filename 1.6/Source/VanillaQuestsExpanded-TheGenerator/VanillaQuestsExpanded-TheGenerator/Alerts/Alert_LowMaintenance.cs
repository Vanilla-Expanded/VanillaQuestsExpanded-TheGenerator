using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Alert_LowMaintenance : Alert
    {
        public Alert_LowMaintenance()
        {
            defaultPriority = AlertPriority.High;
            defaultLabel = "VQE_Alert_LowMaintenance".Translate();
            defaultExplanation = "VQE_Alert_LowMaintenance_Desc".Translate();
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

            return AlertReport.CulpritsAre(GetLowMaintenance(map).ToList());
        }

        public static IEnumerable<Thing> GetLowMaintenance(Map map)
        {
            var genetrons = map.GetComponent<Genetron_MapComponent>().maintainables_InMap;

            foreach (var genetron in genetrons)
            {
                float maintenance = ((Building_GenetronWithMaintenance)genetron).maintenance;
                if (maintenance > 0.3f || maintenance<=0.1f)
                {
                     continue;
                }else
      
                    yield return genetron;
                
               
            }
        }
    }
}
