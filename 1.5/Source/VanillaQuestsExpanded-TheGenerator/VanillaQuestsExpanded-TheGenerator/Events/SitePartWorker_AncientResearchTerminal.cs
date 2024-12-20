using RimWorld;
using RimWorld.Planet;
using System.Linq;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class SitePartWorker_AncientResearchTerminal : SitePartWorker
    {
        public override void Init(Site site, SitePart sitePart)
        {
            base.Init(site, sitePart);
            sitePart.things = new ThingOwner<Thing>(sitePart);
            Thing item = ThingMaker.MakeThing(InternalDefOf.VQE_AncientResearchTerminal);
            sitePart.things.TryAdd(item);
        }

        public override void PostMapGenerate(Map map)
        {
            base.PostMapGenerate(map);
            var terminals = map.listerThings.ThingsOfDef(InternalDefOf.VQE_AncientResearchTerminal);
            if (terminals.TryRandomElement(out var terminal))
            {
                var site = map.Parent as Site;
                Thing terminal2 = site.parts[0].things.FirstOrDefault(t => t.def == InternalDefOf.VQE_AncientResearchTerminal);
                var pos = terminal.Position;
                var rot = terminal.Rotation;
                terminal.Destroy();
                GenSpawn.Spawn(terminal2, pos, map, rot);
            }
        }

        public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
        {
            var comp = map.Parent.GetComponent<TimedMakeFactionHostile>();
            if (comp.TicksLeft.HasValue)
            {
                return base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets).Formatted(comp.TicksLeft.Value.ToStringTicksToPeriod().Named("TIMER"));
            }
            return base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
        }
    }
}
