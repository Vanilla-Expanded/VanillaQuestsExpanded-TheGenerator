using RimWorld;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class GameCondition_UnnaturalColdSnap : GameCondition
    {
        public override WeatherDef ForcedWeather()
        {
            return InternalDefOf.SnowHard;
        }

        public override float TemperatureOffset()
        {
            return -120f;
        }
    }
}
