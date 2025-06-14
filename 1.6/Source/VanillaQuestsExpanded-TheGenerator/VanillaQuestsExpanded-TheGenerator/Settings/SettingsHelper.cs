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
    public class SettingsHelper
    {
        public static void HorizontalSliderLabeled(Rect rect, ref float value, FloatRange range, string leftLabel, string righLabel, string label = null, float roundTo = -1f)
        {
            float trueMin = range.TrueMin;
            float trueMax = range.TrueMax;
            value = Widgets.HorizontalSlider(rect, value, trueMin, trueMax, middleAlignment: false, label, leftLabel, righLabel, roundTo);
        }



    }
}
