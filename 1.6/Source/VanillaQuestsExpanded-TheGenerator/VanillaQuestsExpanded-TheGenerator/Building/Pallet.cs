using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.AI;
using static UnityEngine.Random;
using Verse.Noise;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Pallet : Building, IOpenable
    {
        
        public virtual int OpenTicks => 300;

        public virtual bool CanOpen => true;

        public virtual void Open()
        {
            PalletContents contentDetails = this.def.GetModExtension<PalletContents>();
            if (contentDetails != null)
            {
                foreach(ThingAndCount thingDefCount in contentDetails.contents)
                {
                    Thing thingToMake = ThingMaker.MakeThing(thingDefCount.thing,null);
                    thingToMake.stackCount = thingDefCount.count;
                    GenPlace.TryPlaceThing(thingToMake, Position, Map, ThingPlaceMode.Near);

                    
                }
                Thing palletToMake = GenSpawn.Spawn(ThingMaker.MakeThing(InternalDefOf.VQE_EmptyConstructionPallet), Position, Map);
              
                if (palletToMake.def.CanHaveFaction)
                {
                    palletToMake.SetFaction(this.Faction);
                }
                if (this.Spawned)
                {
                    this.Destroy();
                }

            }


        }

    }
}
