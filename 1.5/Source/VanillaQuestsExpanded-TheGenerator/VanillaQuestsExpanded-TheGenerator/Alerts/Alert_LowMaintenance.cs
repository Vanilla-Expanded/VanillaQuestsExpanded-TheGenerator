using System.Collections.Generic;
using System.Linq;

using RimWorld;

using Verse;

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
                if (maintenance > 0.3f || maintenance<0.1f)
                {
                     continue;
                }else
      
                    yield return genetron;
                
               
            }
        }
    }
}
