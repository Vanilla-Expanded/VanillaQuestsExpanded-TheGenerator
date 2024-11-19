using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronOverdrive : Building_Genetron
    {

        private IntermittentSteamSprayer_Constant steamSprayer;
        public bool overdrive = false;
        public const int overdriveTime = 300000; // 5 days
        public int overdriveTimer = 0;
        public bool overdriveCanBeReUsed = true;
        public int overdriveCanBeReUsedTime = 1800000; // 30 days
        public int overdriveCanBeReUsedTimer = 0;
        public bool criticalBreakdown = false;
        public CompRefuelableWithOverdrive compRefuelableWithOverdrive;
        public bool completedOverdriveSuccessfully = false;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref this.overdrive, "overdrive", false, false);
            Scribe_Values.Look(ref this.overdrive, "overdriveCanBeReUsed", true, false);
            Scribe_Values.Look(ref this.overdriveTimer, "overdriveTimer", 0, false);
            Scribe_Values.Look(ref this.overdriveCanBeReUsedTimer, "overdriveCanBeReUsedTimer", 0, false);
            Scribe_Values.Look(ref this.criticalBreakdown, "criticalBreakdown", false, false);
            Scribe_Values.Look(ref this.completedOverdriveSuccessfully, "completedOverdriveSuccessfully", false, false);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            steamSprayer = new IntermittentSteamSprayer_Constant(this);
            compRefuelableWithOverdrive = this.TryGetComp<CompRefuelableWithOverdrive>();
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);

            if (criticalBreakdown)
            {
                Vector3 drawPos = DrawPos + new Vector3(0, 0, 2.6f);
                drawPos.y = AltitudeLayer.MetaOverlays.AltitudeFor() + 0.181818187f;
                float num = ((float)Math.Sin((double)((Time.realtimeSinceStartup + 397f * (float)(thingIDNumber % 571)) * 4f)) + 1f) * 0.5f;
                num = 0.3f + num * 0.7f;
                Material material = FadedMaterialPool.FadedVersionOf(GraphicsCache.CriticalBreakdown, num);
                Graphics.DrawMesh(MeshPool.plane08, drawPos, Quaternion.identity, material, 0);
            }
        }

        public override void Tick()
        {
            base.Tick();


            if (overdrive)
            {
                steamSprayer.SteamSprayerTick();
                overdriveTimer++;
                if (overdriveTimer > overdriveTime)
                {
                    Signal_OverdriveEnded();
                    completedOverdriveSuccessfully = true;
                }
                if (compRefuelable?.HasFuel == false)
                {
                    Signal_OverdriveEnded();
                    Signal_CriticalBreakdown();
                }
            }
            if (!overdriveCanBeReUsed)
            {
                overdriveCanBeReUsedTimer++;
                if (overdriveCanBeReUsedTimer > overdriveCanBeReUsedTime)
                {
                    Signal_OverdriveOffCooldown();
                }
            }
        }

        public void Signal_OverdriveStarted()
        {
            overdrive = true;
            overdriveCanBeReUsed = false;
            CompGlower compGlower = this.TryGetComp<CompGlower>();
            if (compGlower != null)
            {
                compGlower.GlowRadius = 30;
                compGlower.GlowColor = new ColorInt(217, 112, 33, 100);
                Map.mapDrawer.MapMeshDirty(Position, MapMeshFlagDefOf.Things);
            }

        }

        public void Signal_OverdriveEnded()
        {
            overdrive = false;
            overdriveTimer = 0;
            CompGlower compGlower = this.TryGetComp<CompGlower>();
            if (compGlower != null)
            {
                compGlower.GlowRadius = compGlower.Props.glowRadius;
                compGlower.GlowColor = compGlower.Props.glowColor;
                Map.mapDrawer.MapMeshDirty(Position, MapMeshFlagDefOf.Things);
            }
        }

        public void Signal_OverdriveOffCooldown()
        {
            overdriveCanBeReUsed = true;
            overdriveCanBeReUsedTimer = 0;
        }

        public void Signal_CriticalBreakdown()
        {
            GenExplosion.DoExplosion(Position + IntVec3.North * 4, Map, 4.9f, DamageDefOf.Flame, this, -1, -1f);
            GenExplosion.DoExplosion(Position + IntVec3.South * 4, Map, 4.9f, DamageDefOf.Flame, this, -1, -1f);
            GenExplosion.DoExplosion(Position + IntVec3.West * 4, Map, 4.9f, DamageDefOf.Flame, this, -1, -1f);
            GenExplosion.DoExplosion(Position + IntVec3.East * 4, Map, 4.9f, DamageDefOf.Flame, this, -1, -1f);
            criticalBreakdown = true;
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.AddRepairableToMap(this);
            }
            
            CellRect cellRect = GenAdj.OccupiedRect(Position, Rot4.North, def.Size);
            int randomAmountOfChunks = Rand.RangeInclusive(3, 9);
            for(int i=0;i<randomAmountOfChunks;i++)
            {
                IntVec3 randomCell = cellRect.RandomCell;

                if (RCellFinder.TryFindRandomCellNearWith(Position, (IntVec3 c) => !c.Fogged(Map) && c.Walkable(Map) && !c.Impassable(Map), Map, out IntVec3 result, 13, 28))
                {
                    var projectile = (Projectile)GenSpawn.Spawn(InternalDefOf.VQE_ChunkProjectile, randomCell, Map);
                    projectile.Launch(this, result, result, ProjectileHitFlags.None, false, null);
                }
            }
        }

        public void Signal_CriticalBreakdownRepaired()
        {
            criticalBreakdown = false;
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveRepairableFromMap(this);
            }
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();

            if (overdriveCanBeReUsed)
            {
                command_Action.defaultDesc = "VQE_GenetronOverdriveDesc".Translate();
                command_Action.defaultLabel = "VQE_GenetronOverdrive".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/GeneratorOverdrive_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Signal_OverdriveStarted();
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_GenetronOverdriveDescExtended".Translate((overdriveTime - overdriveTimer).ToStringTicksToPeriod());
                command_Action.defaultLabel = "VQE_GenetronOverdrive".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/GeneratorOverdrive_Gizmo", true);
                command_Action.Disabled = true;
            }
            yield return command_Action;

            if (DebugSettings.ShowDevGizmos)
            {
                Command_Action command_Action3 = new Command_Action();
                command_Action3.defaultLabel = "Reset overdrive cooldown";
                command_Action3.action = delegate
                {
                    Signal_OverdriveOffCooldown();
                };
                yield return command_Action3;
                Command_Action command_Action4 = new Command_Action();
                command_Action4.defaultLabel = "Stop overdrive";
                command_Action4.action = delegate
                {
                    Signal_OverdriveEnded();
                };
                yield return command_Action4;
                Command_Action command_Action5 = new Command_Action();
                command_Action5.defaultLabel = "Set overdrive successful for 5 days";
                command_Action5.action = delegate
                {
                    completedOverdriveSuccessfully = true;
                };
                yield return command_Action5;
            }
        }

        public override string GetInspectString()
        {
            if (criticalBreakdown)
            {
                return base.GetInspectString() +  "VQE_CriticalBreakdown".Translate();
            }
            return base.GetInspectString();
        }






    }
}
