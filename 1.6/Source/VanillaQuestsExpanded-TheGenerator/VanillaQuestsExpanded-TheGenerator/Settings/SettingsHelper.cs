using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    [StaticConstructorOnStartup]
    public static class SettingsHelper
    {
        public static void HorizontalSliderLabeled(Rect rect, ref float value, FloatRange range, string leftLabel, string righLabel, string label = null, float roundTo = -1f)
        {
            float trueMin = range.TrueMin;
            float trueMax = range.TrueMax;
            value = Widgets.HorizontalSlider(rect, value, trueMin, trueMax, middleAlignment: false, label, leftLabel, righLabel, roundTo);
        }

        public static bool Settings_Button(this Listing_Standard ls, string label, Rect rect)
        {

            bool result = Widgets.ButtonText(rect, label, true, true, true);

            ls.Gap((2f));
            return result;

        }

        public static Rect LabelPlusButton(this Listing_Standard ls, string label, string tooltip = null)
        {
            float num = Text.CalcHeight(label, ls.ColumnWidth);

            Rect rect = ls.GetRect(num);

            Widgets.Label(rect, label);

            if (tooltip != null)
            {
                TooltipHandler.TipRegion(rect, tooltip);
            }
            ls.Gap(50);
            return rect;
        }

    }
}
