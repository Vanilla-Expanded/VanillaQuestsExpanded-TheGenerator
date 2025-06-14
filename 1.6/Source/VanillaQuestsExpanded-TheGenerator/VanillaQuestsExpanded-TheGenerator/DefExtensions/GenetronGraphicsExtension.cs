using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{

    public class GenetronGraphicsExtension : DefModExtension
    {

        public List<GenetronGraphics> graphics = null;
       

    }

    public class GenetronGraphics
    {
        public string texture;
        public Vector2 size = Vector2.zero;
        public Vector2 offset = Vector2.zero;
        public bool rotation = false;

    }

}
