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
    public class Building_Genetron_Studiable : Building_Genetron_Base
    {
        Genetron_MapComponent comp;
      
        public string studyStartedSignal;
        public string studyFinishedSignal;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            comp = Map.GetComponent<Genetron_MapComponent>();
        }


        public override void ExposeData()
        {
            base.ExposeData();
          
            Scribe_Values.Look(ref studyStartedSignal, "studyStartedSignal");
            Scribe_Values.Look(ref studyFinishedSignal, "studyFinishedSignal");
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();

            if (comp?.studiables_InMap.Contains(this)==false)
            {
                command_Action.defaultDesc = "VQE_StudyAncientGenetronDesc".Translate(this.def.label);
                if (cachedDetailsExtension != null && cachedDetailsExtension.descriptionOverride.NullOrEmpty() is false)
                {
                    command_Action.defaultDesc = cachedDetailsExtension.descriptionOverride.ToString();
                }
                command_Action.defaultLabel = "VQE_StudyAncientGenetron".Translate(this.def.label);
                command_Action.icon = ContentFinder<Texture2D>.Get(cachedDetailsExtension.studyGizmoIcon, true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Map.GetComponent<Genetron_MapComponent>()?.AddStudiablesToMap(this);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQE_StudyAncientGenetronDesc".Translate(this.def.label);
                if (cachedDetailsExtension != null && cachedDetailsExtension.descriptionOverride.NullOrEmpty() is false)
                {
                    command_Action.defaultDesc = cachedDetailsExtension.descriptionOverride.ToString();
                }
                command_Action.defaultDescPostfix = "VQE_StudyAncientGenetronDescExtended".Translate(this.def.label).Colorize(Utils.tooltipColour);
                command_Action.defaultLabel = "VQE_StudyAncientGenetron".Translate(this.def.label);
                command_Action.icon = ContentFinder<Texture2D>.Get(cachedDetailsExtension.studyGizmoIcon, true);
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

            if (comp?.studiables_InMap.Contains(this) == true)
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
            if (selPawn.CanReserve(this) && selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)
                && !selPawn.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled)
            {
                if (!selPawn.CanReach(this, PathEndMode.OnCell, Danger.Deadly))
                {
                    yield return new FloatMenuOption("CannotUseReason".Translate("NoPath".Translate().CapitalizeFirst()), null);
                }
                else
                {
                    yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("VQE_StudyAncientGenetron".Translate(this.def.label).CapitalizeFirst(), delegate
                    {
                        selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.VQE_StudyGenetron, this), JobTag.Misc);
                    }), selPawn, this);
                }
            }
            
        }






    }
}
