using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.Sound;
using HarmonyLib;
using Verse.Noise;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronOverdrive : Building_Genetron
    {

        public Genetron_Overdrive_SteamSprayer overdriveSprayer;
        public Genetron_BlackSteam_SteamSprayer smokeSprayer;
        public bool overdrive = false;
        public const int overdriveTime = 300000; // 5 days
        public int overdriveTimer = 0;
        public bool overdriveCanBeReUsed = true;
        public const int overdriveCanBeReUsedTime = 1800000; // 30 days
        public int overdriveCanBeReUsedTimer = 0;
        public bool criticalBreakdown = false;
        public CompRefuelableWithOverdrive compRefuelableWithOverdrive;
        public bool completedOverdriveSuccessfully = false;

        //Nuclear meltdown stuff

        public bool inMeltdown = false;
        public const int ticksSmoke = 600; //10 seconds
        public int ticksSmokeTimer = 0;
        public bool wickActive = false;
        protected Sustainer wickSoundSustainer;
        public int wickTicksLeft;
        public int wickTimer=0;
        public OverlayHandle? overlayBurningWick;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref this.overdrive, "overdrive", false, false);
            Scribe_Values.Look(ref this.overdrive, "overdriveCanBeReUsed", true, false);
            Scribe_Values.Look(ref this.overdriveTimer, "overdriveTimer", 0, false);
            Scribe_Values.Look(ref this.overdriveCanBeReUsedTimer, "overdriveCanBeReUsedTimer", 0, false);
            Scribe_Values.Look(ref this.criticalBreakdown, "criticalBreakdown", false, false);
            Scribe_Values.Look(ref this.completedOverdriveSuccessfully, "completedOverdriveSuccessfully", false, false);
            Scribe_Values.Look(ref this.inMeltdown, "inMeltdown", false, false);
            Scribe_Values.Look(ref this.ticksSmokeTimer, "ticksSmokeTimer", 0, false);
            Scribe_Values.Look(ref this.wickTicksLeft, "wickTicksLeft", 0, false);
            Scribe_Values.Look(ref this.wickActive, "wickActive", false, false);
            Scribe_Values.Look(ref this.wickTimer, "wickTimer", 0, false);


        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            overdriveSprayer = new Genetron_Overdrive_SteamSprayer(this);
            smokeSprayer = new Genetron_BlackSteam_SteamSprayer(this);
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

        protected override void Tick()
        {
            base.Tick();


            if (overdrive)
            {
                overdriveSprayer.SteamSprayerTick();
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
            if (inMeltdown)
            {
                smokeSprayer.SteamSprayerTick();

                ticksSmokeTimer++;
                if ((ticksSmokeTimer > ticksSmoke)&& !wickActive)
                {
                    Signal_NuclearCriticalBreakdown_Fuse();
                }
            }
            if (wickActive)
            {
                if (wickSoundSustainer != null)
                {                   
                    wickSoundSustainer.Maintain();
                }
                wickTimer++;
                if (wickTimer > wickTicksLeft)
                {
                    if (cachedDetailsExtension.hasNuclearMeltdowns) {

                        Signal_NuclearCriticalBreakdown_Detonate();
                    }
                    else
                    {
                        Signal_CriticalBreakdown();
                    }
                    
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
            criticalBreakdown = true;
            Genetron_MapComponent mapComp = Map.GetComponent<Genetron_MapComponent>();
            if (mapComp != null)
            {
                mapComp.AddRepairableToMap(this);
            }
            Signal_Explode();
        }

        public void Signal_NuclearCriticalBreakdown_Begin()
        {
            inMeltdown = true;
        }
        public void Signal_NuclearCriticalBreakdown_Fuse()
        {
            wickTicksLeft = (new IntRange(1200, 3600)).RandomInRange;
           
            SoundInfo info = SoundInfo.InMap(this, MaintenanceType.PerTick);
            wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
            Map.overlayDrawer.Disable(this,ref overlayBurningWick);
            overlayBurningWick = Map.overlayDrawer.Enable(this, OverlayTypes.BurningWick);
            wickActive = true;
            
        }
        public void Signal_ShortFuse()
        {
            wickTicksLeft = 1200;
            SoundInfo info = SoundInfo.InMap(this, MaintenanceType.PerTick);
            wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
            Map.overlayDrawer.Disable(this, ref overlayBurningWick);
            overlayBurningWick = Map.overlayDrawer.Enable(this, OverlayTypes.BurningWick);
            wickActive = true;

        }
        public void Signal_NuclearCriticalBreakdown_Detonate()
        {
            if (wickSoundSustainer != null)
            {
                wickSoundSustainer.End();
                wickSoundSustainer = null;
            }
            wickActive = false;
            InternalDefOf.VQE_MeltdownExplosion.PlayOneShotOnCamera();

            CellRect cellRect = GenAdj.OccupiedRect(Position, Rot4.North, def.Size);
            int randomAmountOfChunks = Rand.RangeInclusive(9, 12);
            for (int i = 0; i < randomAmountOfChunks; i++)
            {
                IntVec3 randomCell = cellRect.RandomCell;

                if (RCellFinder.TryFindRandomCellNearWith(Position, (IntVec3 c) => !c.Fogged(Map) && c.Walkable(Map) && !c.Impassable(Map), Map, out IntVec3 result, 13, 60))
                {
                    var projectile = (Projectile)GenSpawn.Spawn(InternalDefOf.VQE_ChunkProjectile, randomCell, Map);
                    projectile.Launch(this, result, result, ProjectileHitFlags.None, false, null);
                }
            }

            int radius = Math.Min(Mathf.CeilToInt(compRefuelable.Fuel),282);

            CellRect cells = CellRect.CenteredOn(PositionHeld, radius);

            Find.CameraDriver.shaker.DoShake(mag: 20f);

            AccessTools.FieldRef<MoteCounter, int> moteCount = AccessTools.FieldRefAccess<MoteCounter, int>(fieldName: "moteCount");

            DamageInfo destroyInfo = new DamageInfo(DamageDefOf.Bomb, float.MaxValue, float.MaxValue, instigator: this);
            GenExplosion.DoExplosion(PositionHeld, this.Map, radius, DamageDefOf.Flame, damAmount: 500, applyDamageToExplosionCellsNeighbors: true, chanceToStartFire: 1f, instigator: this, postExplosionSpawnThingDef: InternalDefOf.Filth_Ash, postExplosionSpawnChance: 0.5f);

            int x = 0;
            foreach (IntVec3 intVec3 in cells)
            {
                if (intVec3.InBounds(Map)) {
                    x++;
                    if (x % 50 == 0)
                    {
                        moteCount(this.Map.moteCounter) = 0;
                        Vector3 vc = intVec3.ToVector3();
                        FleckMaker.ThrowLightningGlow(vc, this.Map, size: 10f);
                        FleckMaker.ThrowMetaPuff(vc, this.Map);
                    }
                    List<Thing> things = this.Map.thingGrid.ThingsListAtFast(intVec3);

                    for (int i = 0; i < things.Count; i++)
                    {
                        Thing thing = things[i];
                        if (thing.def.filth is null && thing != this && thing.def != InternalDefOf.VQE_NuclearGenetronHusk && !(thing.def.building?.isNaturalRock ?? false))
                            thing.TakeDamage(destroyInfo);
                    }

                }
                
            }

            FloodFillerFog.FloodUnfog(PositionHeld, Map);

            GenExplosion.DoExplosion(Position + IntVec3.North * 4, Map, radius, DamageDefOf.Flame, damAmount: 500, applyDamageToExplosionCellsNeighbors: true, chanceToStartFire: 1f, instigator: this, postExplosionSpawnThingDef: InternalDefOf.Filth_Ash, postExplosionSpawnChance: 0.5f);
            GenExplosion.DoExplosion(Position + IntVec3.South * 4, Map, radius, DamageDefOf.Flame, damAmount: 500, applyDamageToExplosionCellsNeighbors: true, chanceToStartFire: 1f, instigator: this, postExplosionSpawnThingDef: InternalDefOf.Filth_Ash, postExplosionSpawnChance: 0.5f);
            GenExplosion.DoExplosion(Position + IntVec3.West * 4, Map, radius, DamageDefOf.Flame, damAmount: 500, applyDamageToExplosionCellsNeighbors: true, chanceToStartFire: 1f, instigator: this, postExplosionSpawnThingDef: InternalDefOf.Filth_Ash, postExplosionSpawnChance: 0.5f);
            GenExplosion.DoExplosion(Position + IntVec3.East * 4, Map, radius, DamageDefOf.Flame, damAmount: 500, applyDamageToExplosionCellsNeighbors: true, chanceToStartFire: 1f, instigator: this, postExplosionSpawnThingDef: InternalDefOf.Filth_Ash, postExplosionSpawnChance: 0.5f);

            Thing thingToMake = GenSpawn.Spawn(ThingMaker.MakeThing(InternalDefOf.VQE_NuclearGenetronHusk), PositionHeld, Map);

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
            if(cachedDetailsExtension?.hideOverdrive != true)
            {
                Command_Action command_Action = new Command_Action();

                if (overdriveCanBeReUsed)
                {
                    command_Action.defaultDesc = "VQE_GenetronOverdriveDesc".Translate(overdriveTime.ToStringTicksToPeriod());
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
                    command_Action.defaultDesc = "VQE_GenetronOverdriveDesc".Translate(overdriveTime.ToStringTicksToPeriod())+"VQE_GenetronOverdriveDescExtended".Translate(overdriveCanBeReUsedTime.ToStringTicksToPeriod(),(overdriveCanBeReUsedTime - overdriveCanBeReUsedTimer).ToStringTicksToPeriod());
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
                    if (cachedDetailsExtension?.hasNuclearMeltdowns == true)
                    {
                        Command_Action command_Action6 = new Command_Action();
                        command_Action6.defaultLabel = "Nuclear meltdown";
                        command_Action6.action = delegate
                        {
                            Signal_NuclearCriticalBreakdown_Begin();
                        };
                        yield return command_Action6;
                    }
                }

            }
                    
        }

        public override string GetInspectString()
        {
            if (criticalBreakdown)
            {
                return base.GetInspectString() +  "\n"+"VQE_CriticalBreakdown".Translate();
            }
            return base.GetInspectString();
        }






    }
}
