using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;

using Verse;
using Verse.Noise;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Window_Tuning : Window
    {


        public override Vector2 InitialSize => new Vector2(500f, 180f);
        private Vector2 scrollPosition = new Vector2(0, 0);

        Building_GenetronTuning building;
      
        private static readonly Color borderColor = new Color(0.13f, 0.13f, 0.13f);
        private static readonly Color fillColor = new Color(0, 0, 0, 0.1f);

        public Window_Tuning(Building_GenetronTuning building)
        {

            this.building = building;
            draggable = false;
            resizeable = false;
            preventCameraMotion = false;

        }

      
        public override void DoWindowContents(Rect inRect)
        {

            var outRect = new Rect(inRect);
            outRect.yMin += 40f;
            outRect.yMax -= 40f;
            outRect.width -= 16f;

            Text.Font = GameFont.Medium;
            var IntroLabel = new Rect(0, 0, 300, 32f);
            Widgets.Label(IntroLabel, "VQE_GenetronTuning".Translate().CapitalizeFirst());
            Text.Font = GameFont.Small;
            var IntroLabel2 = new Rect(0, 40, 300, 32f);
            Widgets.Label(IntroLabel2, "VQE_GenetronTuningProjected".Translate((0f - building.compPower.Props.PowerConsumption) * building.compRefuelableWithOverdrive.tuningMultiplier).CapitalizeFirst());
            var IntroLabel3 = new Rect(0, 60, 300, 32f);       
            Widgets.Label(IntroLabel3, "VQE_GenetronTuningSet".Translate(building.compRefuelableWithOverdrive.tuningMultiplier * 100).CapitalizeFirst());

            if (Widgets.ButtonImage(new Rect(outRect.xMax - 18f - 4f, 2f, 18f, 18f), TexButton.CloseXSmall))
            {
               
                

                Close();
            }
            var SliderContainer1 = new Rect(0, 120, 450, 32f);
            SettingsHelper.HorizontalSliderLabeled(SliderContainer1, ref building.compRefuelableWithOverdrive.tuningMultiplier, new FloatRange(0,2),"0%","200%");

        }
    }
}