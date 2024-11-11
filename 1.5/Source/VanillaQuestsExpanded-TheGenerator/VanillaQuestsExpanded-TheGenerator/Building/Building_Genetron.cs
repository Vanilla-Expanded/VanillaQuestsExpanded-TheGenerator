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
        public Dictionary<Graphic_Single,Tuple<bool,Vector2>> cachedGraphics = new Dictionary<Graphic_Single, Tuple<bool, Vector2>>();
        private float spinPosition;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            cachedExtension = this.def.GetModExtension<GenetronGraphicsExtension>();
            if (cachedExtension != null)
            {
                LongEventHandler.ExecuteWhenFinished(delegate { StoreGraphics(); });

            }

            List<Thing> list = map.thingGrid.ThingsListAt(Position);
            for (int i = 0; i < list.Count; i++)
            {
                Building existingBuilding = list[i] as Building;
                if (existingBuilding != this && existingBuilding is Building_Genetron)
                {
                    existingBuilding.Destroy();
                }
            }

            
            

        }

        public void StoreGraphics()
        {
            foreach (GenetronGraphics graphic in cachedExtension.graphics)
            {             
                cachedGraphics.Add((Graphic_Single)GraphicDatabase.Get<Graphic_Single>(graphic.texture, ShaderDatabase.Cutout, 
                    graphic.size != Vector2.zero ? graphic.size : this.def.graphicData.drawSize, Color.white),Tuple.Create(graphic.rotation,graphic.offset));
            }

        }


        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);

            float speed = 0.75f;
            float extrarot = (Current.Game.tickManager.TicksGame * speed / Mathf.PI);

            if (!cachedGraphics.NullOrEmpty()) {

                var vector = DrawPos + Altitudes.AltIncVect;
                foreach(KeyValuePair<Graphic_Single, Tuple<bool, Vector2>> overlay in cachedGraphics)
                {
                    vector.y += Altitudes.AltInc;

                    Vector3 vectorOffset = Vector3.zero;

                    if (overlay.Value.Item2 != Vector2.zero)
                    {
                        vectorOffset.x = overlay.Value.Item2.x;
                        vectorOffset.z = overlay.Value.Item2.y;
                    }

                    overlay.Key.DrawFromDef(vector + vectorOffset, Rot4.North, null, overlay.Value.Item1 ? extrarot : 0);

                 
                                     
                }
                
            }          

        }







    }
}
