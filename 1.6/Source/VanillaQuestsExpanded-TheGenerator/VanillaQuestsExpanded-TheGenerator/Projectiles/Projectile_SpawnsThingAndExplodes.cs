using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Projectile_SpawnsThingAndExplodes : Projectile
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            Map map = this.Map;
           

           
          
            IntVec3 loc = base.Position;
            List<Thing> thingsInImpact = base.Position.GetThingList(map);
            List<Thing> thingsToDestroy = new List<Thing>();
            foreach (Thing thingImpacted in thingsInImpact)
            {
                if (thingImpacted.def != InternalDefOf.VQE_ChunkProjectile && thingImpacted.def != InternalDefOf.VQE_GenetronJunk)
                {
                    thingsToDestroy.Add(thingImpacted);
                    
                }
            }
            foreach(Thing thingToDestroy in thingsToDestroy)
            {
                thingToDestroy.Destroy(DestroyMode.KillFinalize);
            }
            GenExplosion.DoExplosion(loc, map, 2.9f, DamageDefOf.Flame, this, -1, -1, null, null, null, null, null, 0f, 1, null,null,255, false, null, 0f, 1);

            Thing thingToMake = GenSpawn.Spawn(ThingMaker.MakeThing(def.projectile.spawnsThingDef), loc, map);
            if (thingToMake.def.CanHaveFaction)
            {
                thingToMake.SetFaction(base.Launcher.Faction);
            }
            Destroy();
        }

        protected override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(10))
            {
                ThrowBlackSmoke(this.DrawPos, this.Map, 4);
            }
            

        }

        private void ThrowBlackSmoke(Vector3 loc, Map map, float size)
        {
            if (!loc.ShouldSpawnMotesAt(map))
                return;

            FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.Smoke, Rand.Range(1.5f, 2.5f) * size);
            dataStatic.rotationRate = Rand.Range(-30f, 30f);
            dataStatic.velocityAngle = Rand.Range(30, 40);
            dataStatic.velocitySpeed = Rand.Range(0.5f, 0.7f);
            dataStatic.instanceColor = Color.black;
            map.flecks.CreateFleck(dataStatic);
        }



    }
}