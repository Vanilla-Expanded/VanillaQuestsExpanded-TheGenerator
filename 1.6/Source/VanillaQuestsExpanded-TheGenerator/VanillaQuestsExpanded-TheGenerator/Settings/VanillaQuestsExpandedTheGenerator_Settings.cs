using RimWorld;
using System;
using UnityEngine;
using Verse;


namespace VanillaQuestsExpandedTheGenerator
{
    public class VanillaQuestsExpandedTheGenerator_Settings : ModSettings

    {
      

        public static bool disableNeedForSteamVent = false;       

        public static float ARCTimersMultiplier = ARCTimersMultiplierBase;
        public const float ARCTimersMultiplierBase = 1f;

        public static float ARCFuelMultiplier = ARCFuelMultiplierBase;
        public const float ARCFuelMultiplierBase = 1f;

        public static float ARCOutputMultiplier = ARCOutputMultiplierBase;
        public const float ARCOutputMultiplierBase = 1f;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref disableNeedForSteamVent, "disableNeedForSteamVent", false, true);
          
            Scribe_Values.Look(ref ARCTimersMultiplier, "ARCTimersMultiplier", ARCTimersMultiplierBase, true);
            Scribe_Values.Look(ref ARCFuelMultiplier, "ARCFuelMultiplier", ARCFuelMultiplierBase, true);
            Scribe_Values.Look(ref ARCOutputMultiplier, "ARCOutputMultiplier", ARCOutputMultiplierBase, true);


        }
        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();

           
            ls.Begin(inRect);

            ls.CheckboxLabeled("VQEG_DisableNeedForSteamVent".Translate(), ref disableNeedForSteamVent, "VQEG_DisableNeedForSteamVentTooltip".Translate());
         

            ls.GapLine();

            var ARCTimersMultiplierLabel = ls.LabelPlusButton("VQEG_ARCTimersMultiplier".Translate() + ": x" + ARCTimersMultiplier, "VQEG_ARCTimersMultiplierTooltip".Translate());
            ARCTimersMultiplier = (float)Math.Round(ls.Slider(ARCTimersMultiplier, 0.1f, 3), 1);

            if (ls.Settings_Button("VQEG_Reset".Translate(), new Rect(0f, ARCTimersMultiplierLabel.position.y + 35, 250f, 29f)))
            {
                ARCTimersMultiplier = ARCTimersMultiplierBase;
            }
            ls.GapLine();
            var ARCFuelMultiplierLabel = ls.LabelPlusButton("VQEG_ARCFuelMultiplier".Translate() + ": x" + ARCFuelMultiplier, "VQEG_ARCFuelMultiplierTooltip".Translate());
            ARCFuelMultiplier = (float)Math.Round(ls.Slider(ARCFuelMultiplier, 0.1f, 3), 1);

            if (ls.Settings_Button("VQEG_Reset".Translate(), new Rect(0f, ARCFuelMultiplierLabel.position.y + 35, 250f, 29f)))
            {
                ARCFuelMultiplier = ARCFuelMultiplierBase;
            }
            var ARCOutputMultiplierLabel = ls.LabelPlusButton("VQEG_ARCOutputMultiplier".Translate() + ": x" + ARCOutputMultiplier, "VQEG_ARCOutputMultiplierTooltip".Translate());
            ARCOutputMultiplier = (float)Math.Round(ls.Slider(ARCOutputMultiplier, 0.1f, 10), 1);

            if (ls.Settings_Button("VQEG_Reset".Translate(), new Rect(0f, ARCOutputMultiplierLabel.position.y + 35, 250f, 29f)))
            {
                ARCOutputMultiplier = ARCOutputMultiplierBase;
            }


            ls.End();
           
        }




    }



}
