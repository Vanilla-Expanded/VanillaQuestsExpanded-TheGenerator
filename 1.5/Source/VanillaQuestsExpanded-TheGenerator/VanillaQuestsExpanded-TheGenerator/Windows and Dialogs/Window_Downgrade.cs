using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using HarmonyLib;
using RimWorld;
using UnityEngine;

using Verse;
using Verse.Noise;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Window_Downgrade : Window
    {


        public override Vector2 InitialSize => new Vector2(500f, 180f);
        private Vector2 scrollPosition = new Vector2(0, 0);

        Building_Genetron building;
        ThingDef newBuilding;

        private static readonly Color borderColor = new Color(0.13f, 0.13f, 0.13f);
        private static readonly Color fillColor = new Color(0, 0, 0, 0.1f);

        public Window_Downgrade(Building_Genetron building, ThingDef newBuilding)
        {

            this.building = building;
            this.newBuilding = newBuilding;
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

            Text.Font = GameFont.Small;
            var IntroLabel = new Rect(0, 0, 450, 180f);
            Widgets.Label(IntroLabel, "VQE_DowngradeGenetronDialog".Translate(newBuilding.LabelCap));
          
            float num = (inRect.width - 10f) / 2f;
            if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - 30f, num, 30f), "CancelButton".Translate()))
            {
                Close();
            }
            if (Widgets.ButtonText(new Rect(inRect.x + num + 10f, inRect.yMax - 30f, num, 30f), "OK".Translate()))
            {
                if (building is Building_GenetronOverdrive genetronOverdrive && genetronOverdrive.inMeltdown)
                {
                    Messages.Message("VQE_DowngradeGenetronMeltdown".Translate(), building, MessageTypeDefOf.RejectInput, false);
                }
                else
                {
                    Close();
                    Thing thingToMake = GenSpawn.Spawn(ThingMaker.MakeThing(newBuilding), building.PositionHeld, building.Map);
                    thingToMake.SetFaction(building.Faction);
                }
            }
        }
    }
}