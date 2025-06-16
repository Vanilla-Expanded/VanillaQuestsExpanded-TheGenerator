using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class GenStep_NuclearArc : GenStep_Village
    {
        public override void PostGenerate(CellRect rect, Map map, GenStepParams parms)
        {
            base.PostGenerate(rect, map, parms);
            var gameCondition = GameConditionMaker.MakeConditionPermanent(GameConditionDefOf.ToxicFallout);
            map.gameConditionManager.RegisterCondition(gameCondition);
            gameCondition.startTick = Find.TickManager.TicksGame - GenDate.TicksPerDay;
        }
    }
}
