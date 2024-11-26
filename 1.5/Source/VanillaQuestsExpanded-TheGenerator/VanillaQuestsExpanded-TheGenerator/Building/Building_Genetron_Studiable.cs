using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.AI;
using static UnityEngine.Random;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_Studiable : Building
    {
        Genetron_MapComponent comp;
        public ExtraGenetronParameters cachedDetailsExtension;
        public bool alreadyStudied = false;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            comp = Map.GetComponent<Genetron_MapComponent>();
            cachedDetailsExtension = this.def.GetModExtension<ExtraGenetronParameters>();


        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.alreadyStudied, "alreadyStudied", false, false);
         
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();

            if (!alreadyStudied && comp?.studiables_InMap.Contains(this)==false)
            {
                command_Action.defaultDesc = "VQE_StudyAncientGenetronDesc".Translate();
                command_Action.defaultLabel = "VQE_StudyAncientGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/StudyAncientGenetron_Gizmo", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Map.GetComponent<Genetron_MapComponent>()?.AddStudiablesToMap(this);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_StudyAncientGenetronDesc".Translate();
                command_Action.defaultDescPostfix = "VQE_StudyAncientGenetronDescExtended".Translate().Colorize(Utils.tooltipColour);
                command_Action.defaultLabel = "VQE_StudyAncientGenetron".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/StudyAncientGenetron_Gizmo", true);
                command_Action.Disabled = true;                
            }           
            
            yield return command_Action;

        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            
            if (comp != null)
            {
                comp.RemoveStudiablesFromMap(this);
            }
            base.Destroy(mode);

        }

        public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
        {
         
            if (comp != null)
            {
                comp.RemoveStudiablesFromMap(this);
            }
            base.Kill(dinfo, exactCulprit);

        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);

            if (comp?.studiables_InMap.Contains(this) == true && !alreadyStudied)
            {
                Vector3 drawPos = DrawPos;
                drawPos.y = AltitudeLayer.MetaOverlays.AltitudeFor() + 0.181818187f;
                float num = ((float)Math.Sin((double)((Time.realtimeSinceStartup + 397f * (float)(thingIDNumber % 571)) * 4f)) + 1f) * 0.5f;
                num = 0.3f + num * 0.7f;
                Material material = FadedMaterialPool.FadedVersionOf(GraphicsCache.BeingStudied, num);
                Graphics.DrawMesh(MeshPool.plane08, drawPos, Quaternion.identity, material, 0);
            }
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
            {
                yield return floatMenuOption;
            }
            if (!alreadyStudied && selPawn.CanReserve(this) && selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)
                && !selPawn.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled)
            {
                if (!selPawn.CanReach(this, PathEndMode.OnCell, Danger.Deadly))
                {
                    yield return new FloatMenuOption("CannotUseReason".Translate("NoPath".Translate().CapitalizeFirst()), null);
                }
                else
                {
                    yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("VQE_StudyAncientGenetron".Translate().CapitalizeFirst(), delegate
                    {
                        selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.VQE_StudyGenetron, this), JobTag.Misc);
                    }), selPawn, this);
                }
            }
            
        }






    }
}
