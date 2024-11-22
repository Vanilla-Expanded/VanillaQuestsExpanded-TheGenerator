using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronWithEmergencyShutDown : Building_GenetronWithHazardModes
    {
      

        public override void ExposeData()
        {
            base.ExposeData();
           

        }

        

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

           

        }

        
    }
}
