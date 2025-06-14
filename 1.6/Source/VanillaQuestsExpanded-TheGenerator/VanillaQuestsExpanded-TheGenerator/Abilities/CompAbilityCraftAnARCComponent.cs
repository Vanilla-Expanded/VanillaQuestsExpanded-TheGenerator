using System;
using Verse;
using RimWorld;
using VanillaQuestsExpandedTheGenerator;

namespace VanillaQuestsExpandedTheGenerator
{
    public class CompAbilityCraftAnARCComponent : CompAbilityEffect
    {
        public new CompProperties_CraftAnARCComponent Props
        {
            get
            {
                return (CompProperties_CraftAnARCComponent)this.props;
            }
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            IntVec3 cell = target.Cell;

            Thing component = cell.GetThingList(this.parent.pawn.Map).FirstOrFallback();

            if (component == null || component.def != ThingDefOf.ComponentSpacer)
            {
                Messages.Message("VQE_AbilityNeedsComponent".Translate(), MessageTypeDefOf.RejectInput, true);
                this.parent.StartCooldown(30);
            }
            else
            {
                IntVec3 position = component.Position;
                Map map = component.Map;
                int count = component.stackCount;
                ThingDef newThing;

                if (Rand.Chance(0.5f))
                {
                    newThing = ThingDefOf.ComponentIndustrial;
                }
                else
                {
                    newThing = InternalDefOf.VQE_GenetronComponent;
                }
                Thing thing = ThingMaker.MakeThing(newThing);
                thing.stackCount = 1;
                GenPlace.TryPlaceThing(thing, position, map, ThingPlaceMode.Near);

                component.stackCount--;
                if (component.stackCount <= 0)
                {
                    component.Destroy();
                }





            }


            base.Apply(target, dest);

        }
    }
}
