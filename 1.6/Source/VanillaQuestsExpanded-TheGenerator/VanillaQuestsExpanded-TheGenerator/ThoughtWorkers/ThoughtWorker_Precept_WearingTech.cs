
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class ThoughtWorker_Precept_WearingTech : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
           
            if (numberOfHighTechClothes(p)<=0)
            {
                return ThoughtState.Inactive;
            }
            else
            {
                return ThoughtState.ActiveAtStage(0);

            }
        }
        public override float MoodMultiplier(Pawn pawn)
        {
            return numberOfHighTechClothes(pawn);

        }

        public int numberOfHighTechClothes(Pawn pawn)
        {
            int count = 0;
            List<Apparel> wornApparel = pawn.apparel.WornApparel;
            for (int i = 0; i < wornApparel.Count; i++)
            {
                if (wornApparel[i].def.techLevel > TechLevel.Medieval)
                {
                    count++;
                }
            }
            return count;

        }
    }
}
