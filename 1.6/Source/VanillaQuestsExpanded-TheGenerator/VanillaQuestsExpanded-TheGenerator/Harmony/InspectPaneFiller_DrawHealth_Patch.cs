using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaQuestsExpandedTheGenerator
{
    [StaticConstructorOnStartup]
    [HarmonyPatch(typeof(InspectPaneFiller), "DrawHealth")]
    public static class InspectPaneFiller_DrawHealth_Patch
    {
        private static readonly Texture2D BarBGTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(10, 10, 10).ToColor);

        private static readonly Texture2D HealthTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(50, 50, 40).ToColor);

        public static void Postfix(WidgetRow row, Thing t)
        {
            if (t is Building_GenetronWithMaintenance genetron)
            {
                row.Gap(10);
                Rect rect = new Rect(row.FinalX, row.FinalY, 93f, 16f);
                TooltipHandler.TipRegion(rect, "VQE_GenetronMaintenanceTooltip".Translate());
                row.FillableBar(93f, 16f, genetron.maintenance, genetron.maintenance.ToStringPercent(), HealthTex, BarBGTex);
                GUI.color = Color.white;
            }
        }
    }

}
