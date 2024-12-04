using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{

    public class StatPart_ARCMaintenance : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            float factor = 1f;
            if (HasRequiredPrecept(req.Thing))
            {
                factor = FactorByMaintenance(req.Thing);
            }
           
            val *= factor;
        }

        public bool HasRequiredPrecept(Thing thing)
        {
            Pawn pawn = thing as Pawn;
            if (pawn?.Ideo?.HasPrecept(InternalDefOf.VQE_ARCGenerators_Exalted) == true)
            {
                return true;
            }
            return false;
        }

        public float FactorByMaintenance(Thing thing)
        {
            if (thing.Map != null && StaticCollections.ARCmaintenanceInMap.ContainsKey(thing.Map))
            {
                return StaticCollections.ARCmaintenanceInMap[thing.Map] / 100;
            } else return 1f;
          

        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.HasThing && HasRequiredPrecept(req.Thing))
            {
                return "VQE_MaintenanceImpact".Translate() + (": x" + FactorByMaintenance(req.Thing));
            }
            return null;
        }


    }
}
