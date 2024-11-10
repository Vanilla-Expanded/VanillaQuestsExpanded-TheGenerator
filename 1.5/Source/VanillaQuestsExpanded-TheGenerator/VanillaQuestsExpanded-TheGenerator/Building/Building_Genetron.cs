using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron : Building
    {

        public GenetronGraphicsExtension cachedExtension;
        public List<Graphic_Single> cachedGraphics = new List<Graphic_Single>();


        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            cachedExtension = this.def.GetModExtension<GenetronGraphicsExtension>();
            if (cachedExtension != null)
            {
                LongEventHandler.ExecuteWhenFinished(delegate { StoreGraphics(); });

                

            }

        }

        public void StoreGraphics()
        {
            foreach (string texture in cachedExtension.textures)
            {
                Log.Message("Storing "+ texture);
                cachedGraphics.Add((Graphic_Single)GraphicDatabase.Get<Graphic_Single>(texture, ShaderDatabase.Cutout, this.def.graphicData.drawSize, Color.white));

            }


        }


        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);
            if(!cachedGraphics.NullOrEmpty()) {

                var vector = this.DrawPos + Altitudes.AltIncVect;
                foreach(Graphic overlay in cachedGraphics)
                {

                    vector.y += 5;

                   // Vector2 drawingSize = this.def.graphicData.drawSize;
                    
                    overlay?.DrawFromDef(vector, Rot4.South, null);
                }
                
            }
            

        }







    }
}
