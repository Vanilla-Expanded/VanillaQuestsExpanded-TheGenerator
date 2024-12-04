using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron : Building_Genetron_Base
    {

        public GenetronGraphicsExtension cachedGraphicsExtension;
        public Dictionary<Graphic_Single, Tuple<bool, Vector2>> cachedGraphics = new Dictionary<Graphic_Single, Tuple<bool, Vector2>>();
        public CompPowerPlantGenetron compPower;
        public CompRefuelable compRefuelable;
        public CompBreakdownable compBreakdownable;
        public int totalRunningTicks;
        public float totalFuelBurned;
        public float consumptionRatePerTick = 0;
        public Building_SteamGeyser geyser;
        public Genetron_SteamSprayer steamSprayer;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref this.totalRunningTicks, "totalRunningTicks", 0, false);
            Scribe_Values.Look(ref this.totalFuelBurned, "totalFuelBurned", 0, false);
          
        }


        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            compRefuelable = this.TryGetComp<CompRefuelable>();
            compPower = this.TryGetComp<CompPowerPlantGenetron>();
            compBreakdownable = this.TryGetComp<CompBreakdownable>();
            cachedGraphicsExtension = this.def.GetModExtension<GenetronGraphicsExtension>();
            steamSprayer = new Genetron_SteamSprayer(this);


            if (cachedGraphicsExtension != null)
            {
                LongEventHandler.ExecuteWhenFinished(delegate { StoreGraphics(); });
            }

            if (compRefuelable != null)
            {
                consumptionRatePerTick = compRefuelable.Props.fuelConsumptionRate / 60000f;
            }
            List<Thing> list = map.thingGrid.ThingsListAt(Position);
            for (int i = 0; i < list.Count; i++)
            {
                Building existingBuilding = list[i] as Building;
                if (existingBuilding != this && existingBuilding is Building_Genetron existingBuildingAsGenetron)
                {                   

                    if (existingBuildingAsGenetron.compRefuelable != null && compRefuelable!=null)
                    {
                        foreach(ThingDef fuel in existingBuildingAsGenetron.compRefuelable.Props.fuelFilter.AllowedThingDefs)
                        {
                            if (compRefuelable.Props.fuelFilter.AllowedThingDefs.Contains(fuel))
                            {
                                float newCapacity = compRefuelable.Props.fuelCapacity;
                                float existingCapacity = existingBuildingAsGenetron.compRefuelable.Fuel;
                                compRefuelable.Refuel(newCapacity > existingCapacity ? existingCapacity : newCapacity);
                                break;
                            }
                        } 
                    }
                    if (existingBuildingAsGenetron.compPower != null)
                    {
                        compPower.calibrationCounter = existingBuildingAsGenetron.compPower.calibrationCounter;
                    }
                    Building_GenetronNuclear existingBuildingAsNuclearGenetron = existingBuildingAsGenetron as Building_GenetronNuclear;
                    if(existingBuildingAsNuclearGenetron!=null && this.TryGetComp<CompRefuelableWithOverdrive>() is CompRefuelableWithOverdrive compRefuelableWithOverdrive)
                    {
                        compRefuelableWithOverdrive.permanentFuelRodCalibrationMultiplier = existingBuildingAsNuclearGenetron.compRefuelableWithOverdrive.permanentFuelRodCalibrationMultiplier;

                    }

                    existingBuilding.Destroy();
                }
            }

            base.SpawnSetup(map, respawningAfterLoad);
            


        }

        public void Signal_Explode()
        {
            CellRect cellRect = GenAdj.OccupiedRect(Position, Rot4.North, def.Size);
            int randomAmountOfChunks = Rand.RangeInclusive(3, 9);
            for (int i = 0; i < randomAmountOfChunks; i++)
            {
                IntVec3 randomCell = cellRect.RandomCell;

                if (RCellFinder.TryFindRandomCellNearWith(Position, (IntVec3 c) => !c.Fogged(Map) && c.Walkable(Map) && !c.Impassable(Map), Map, out IntVec3 result, 13, 28))
                {
                    var projectile = (Projectile)GenSpawn.Spawn(InternalDefOf.VQE_ChunkProjectile, randomCell, Map);
                    projectile.Launch(this, result, result, ProjectileHitFlags.None, false, null);
                }
            }
            GenExplosion.DoExplosion(Position + IntVec3.North * 4, Map, 4.9f, DamageDefOf.Flame, this, -1, -1f);
            GenExplosion.DoExplosion(Position + IntVec3.South * 4, Map, 4.9f, DamageDefOf.Flame, this, -1, -1f);
            GenExplosion.DoExplosion(Position + IntVec3.West * 4, Map, 4.9f, DamageDefOf.Flame, this, -1, -1f);
            GenExplosion.DoExplosion(Position + IntVec3.East * 4, Map, 4.9f, DamageDefOf.Flame, this, -1, -1f);

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

            steamSprayer.SteamSprayerTick();

            if (geyser == null)
            {
                geyser = (Building_SteamGeyser)Map.thingGrid.ThingAt(Position, ThingDefOf.SteamGeyser);         
                Genetron_GameComponent.Instance.AddGeyserToSuppressed(geyser);
            }

            if (compPower.PowerOn && (compRefuelable is null ||(compRefuelable!=null && compRefuelable.HasFuel))
                && (compBreakdownable is null || (compBreakdownable != null && !compBreakdownable.BrokenDown)))
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

            if (DebugSettings.ShowDevGizmos)
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
                Command_Action command_Action5 = new Command_Action();
                command_Action5.defaultLabel = "Set geothermal studied to TRUE";
                command_Action5.action = delegate
                {
                    Genetron_GameComponent.Instance.geothermalGenetronStudied = true;
                };
                yield return command_Action5;
                Command_Action command_Action6 = new Command_Action();
                command_Action6.defaultLabel = "Set nuclear studied to TRUE";
                command_Action6.action = delegate
                {
                    Genetron_GameComponent.Instance.nuclearGenetronStudied = true;
                };
                yield return command_Action6;


            }


        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (geyser != null)
            {
                Genetron_GameComponent.Instance.RemoveGeyserFromSuppressed(geyser);
            }

            base.Destroy(mode);
        }




    }
}
