using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using static HarmonyLib.Code;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron : Building
    {

        public GenetronGraphicsExtension cachedGraphicsExtension;
        public Dictionary<Graphic_Single, Tuple<bool, Vector2>> cachedGraphics = new Dictionary<Graphic_Single, Tuple<bool, Vector2>>();
        public CompPowerPlantGenetron compPower;
        public CompRefuelable compRefuelable;
        public int totalRunningTicks;
        public float totalFuelBurned;
        public float consumptionRatePerTick = 0;
    

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref this.totalRunningTicks, "totalRunningTicks", 0, false);
            Scribe_Values.Look(ref this.totalFuelBurned, "totalFuelBurned", 0, false);
          
        }


        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            compRefuelable = this.TryGetComp<CompRefuelable>();
            if (compRefuelable != null)
            {
                consumptionRatePerTick = compRefuelable.Props.fuelConsumptionRate / 60000f;
            }
            List<Thing> list = map.thingGrid.ThingsListAt(Position);
            for (int i = 0; i < list.Count; i++)
            {
                Building existingBuilding = list[i] as Building;
                if (existingBuilding != this && existingBuilding is Building_Genetron)
                {
                    if (((Building_Genetron)existingBuilding).compRefuelable != null)
                    {
                        compRefuelable.Refuel(((Building_Genetron)existingBuilding).compRefuelable.Fuel);
                    }
                    existingBuilding.Destroy();
                }
            }

            base.SpawnSetup(map, respawningAfterLoad);
            cachedGraphicsExtension = this.def.GetModExtension<GenetronGraphicsExtension>();
          
            if (cachedGraphicsExtension != null)
            {
                LongEventHandler.ExecuteWhenFinished(delegate { StoreGraphics(); });

            }
            
            compPower = this.TryGetComp<CompPowerPlantGenetron>();

        }




        public void StoreGraphics()
        {
            foreach (GenetronGraphics graphic in cachedGraphicsExtension.graphics)
            {
                cachedGraphics.Add((Graphic_Single)GraphicDatabase.Get<Graphic_Single>(graphic.texture, ShaderDatabase.Cutout,
                    graphic.size != Vector2.zero ? graphic.size : this.def.graphicData.drawSize, Color.white), Tuple.Create(graphic.rotation, graphic.offset));
            }
        }


        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);
            float speed = 0.75f;
            float extrarot = (Current.Game.tickManager.TicksGame * speed / Mathf.PI);
            if (!cachedGraphics.NullOrEmpty())
            {
                var vector = DrawPos + Altitudes.AltIncVect;
                foreach (KeyValuePair<Graphic_Single, Tuple<bool, Vector2>> overlay in cachedGraphics)
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

        public override void Tick()
        {
            base.Tick();

            if (compPower.PowerOn && (compRefuelable is null ||(compRefuelable!=null && compRefuelable.HasFuel)))
            {
                totalRunningTicks++;
            }
            else
            {
                if (totalRunningTicks > 0)
                {
                    totalRunningTicks = 0;
                }
            }
            if (compRefuelable?.HasFuel == true)
            {
                totalFuelBurned += consumptionRatePerTick;
            }
           
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            if (Prefs.DevMode)
            {
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "Set Running Time to 100 days";
                command_Action.action = delegate
                {
                    totalRunningTicks = 6000000;
                };
                yield return command_Action;
                if (compRefuelable!=null)
                {
                    Command_Action command_Action2 = new Command_Action();
                    command_Action2.defaultLabel = "Set fuel burned to 10000";
                    command_Action2.action = delegate
                    {
                        totalFuelBurned = 10000;
                    };
                    yield return command_Action2;
                }
               
               
            }


        }






    }
}
