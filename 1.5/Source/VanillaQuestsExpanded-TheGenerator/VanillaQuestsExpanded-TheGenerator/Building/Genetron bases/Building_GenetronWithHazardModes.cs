using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;


namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_GenetronWithHazardModes : Building_GenetronWithFuelRodCalibration
    {
        public bool hazardMode = false;
        public int hazardModeCounter = 0;
        public const int radiationTicks = 400;
       
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.hazardMode, "hazardMode", false, false);
            Scribe_Values.Look(ref this.hazardModeCounter, "hazardModeCounter", 0, false);


        }

        public void Signal_ToggleHazardMode()
        {
            hazardMode = !hazardMode;

        }

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            
            if (!hazardMode)
            {
                command_Action.defaultDesc = "VQE_SafeModeDesc".Translate();
                command_Action.defaultLabel = "VQE_SafeMode".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/SafeMode_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Signal_ToggleHazardMode();
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_HazardModeyDesc".Translate();
                command_Action.defaultLabel = "VQE_HazardMode".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/HazardMode_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Signal_ToggleHazardMode();
                };
            }
            yield return command_Action;
            if (DebugSettings.ShowDevGizmos)
            {
                Command_Action command_Action3 = new Command_Action();
                command_Action3.defaultLabel = "Set time in hazard mode to 100 days";
                command_Action3.action = delegate
                {
                    hazardModeCounter=6000000;
                };
                yield return command_Action3;
            }

            }

        public override string GetInspectString()
        {
            if (!hazardMode)
            {
                return base.GetInspectString() + "\n" + "VQE_SafeModeInfo".Translate();
            }
            else
            {
                return base.GetInspectString() + "\n" + "VQE_HazardModeInfo".Translate();

            }

        }

        public override void Tick()
        {
            base.Tick();
            if (hazardMode)
            {
                hazardModeCounter++;
                if (this.IsHashIntervalTick(radiationTicks))
                {
                    int num = GenRadial.NumCellsInRadius(compRefuelable.Fuel/2);
                    for (int i = 0; i < num; i++)
                    {
                        AffectCell(PositionHeld + GenRadial.RadialPattern[i]);
                    }

                }
            }
            else
            {
                if (hazardModeCounter != 0)
                {
                    hazardModeCounter = 0;
                }
            }
            
        }

        public void AffectCell(IntVec3 c)
        {
            if (c.InBounds(Map))
            {
                HashSet<Thing> hashSet = new HashSet<Thing>(c.GetThingList(Map));
                if (hashSet != null)
                {
                    foreach (Thing thing in hashSet)
                    {
                        Pawn affectedPawn = thing as Pawn;
                        if (affectedPawn != null && affectedPawn.RaceProps.IsFlesh)
                        {
                            float num = 0.028758334f;
                            num *= 1 - (affectedPawn.GetStatValue(StatDefOf.ToxicResistance, true));
                            if (num != 0f)
                            {
                                float num2 = Mathf.Lerp(0.85f, 1.15f, Rand.ValueSeeded(affectedPawn.thingIDNumber ^ 74374237));
                                num *= num2;
                                HealthUtility.AdjustSeverity(affectedPawn, HediffDefOf.ToxicBuildup, num);
                            }
                        }
                    }
                }
            }
        }



        public override void DrawExtraSelectionOverlays()
        {
            base.DrawExtraSelectionOverlays();
            if (hazardMode)
            {
                GenDraw.DrawCircleOutline(PositionHeld.ToVector3Shifted(), compRefuelable.Fuel/2, SimpleColor.Green);
            }
        }
    }
}
