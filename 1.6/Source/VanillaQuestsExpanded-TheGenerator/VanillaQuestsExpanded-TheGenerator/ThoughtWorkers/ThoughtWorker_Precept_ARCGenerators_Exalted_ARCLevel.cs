
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class ThoughtWorker_Precept_ARCGenerators_Exalted_ARCLevel : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (p.Map?.IsPlayerHome != true)
            {
                return ThoughtState.Inactive;
            }
            if (!StaticCollections.ARClevelInMap.ContainsKey(p.Map) || StaticCollections.ARClevelInMap[p.Map]==0)
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
            if (StaticCollections.ARClevelInMap.ContainsKey(pawn.Map))
            {

                return (StaticCollections.ARClevelInMap[pawn.Map]);
            }
            else return 0;


        }
    }
}
