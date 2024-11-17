using System.Collections.Generic;
using System.Linq;

using RimWorld;

using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Alert_CriticalMaintenance : Alert
    {
        public Alert_CriticalMaintenance()
        {
            defaultPriority = AlertPriority.Critical;
            defaultLabel = "VQE_Alert_CriticalMaintenance".Translate();
            defaultExplanation = "VQE_Alert_CriticalMaintenance_Desc".Translate();
        }

        public override AlertReport GetReport()
        {

            var map = Find.CurrentMap;
            if (map == null)
            {
                return AlertReport.Inactive;
            }

            return AlertReport.CulpritsAre(GetCriticalMaintenance(map).ToList());
        }

        public static IEnumerable<Thing> GetCriticalMaintenance(Map map)
        {
            var genetrons = map.GetComponent<Genetron_MapComponent>().maintainables_InMap;

            foreach (var genetron in genetrons)
            {
                float maintenance = ((Building_GenetronWithMaintenance)genetron).maintenance;
                if (maintenance > 0.1f)
                {
                    continue;
                }
                else

                    yield return genetron;


            }
        }
    }
}
