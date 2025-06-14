
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class ThoughtWorker_Precept_ARCGenerators_Exalted_StudiedARC : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            return Utils.HasAnyStudiedHediffOrBackstory(p);
        }
    }
}
