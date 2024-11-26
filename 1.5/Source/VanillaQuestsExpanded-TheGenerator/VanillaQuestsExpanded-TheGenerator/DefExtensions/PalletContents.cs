using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{

    public class PalletContents : DefModExtension
    {

        public List<ThingAndCount> contents = null;


    }

    public class ThingAndCount
    {
        public ThingDef thing;
        public int count;

    }


}
