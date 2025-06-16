
using RimWorld;
using Verse;
namespace VanillaQuestsExpandedTheGenerator
{
    public class ThoughtWorker_Precept_ARCGenerators_Exalted_Social : ThoughtWorker_Precept_Social
    {
        protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
        {
            return Utils.HasAnyStudiedHediffOrBackstory(otherPawn);
        }
    }
}
