using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class VanillaQuestsExpandedTheGenerator_Mod : Mod
    {


        public VanillaQuestsExpandedTheGenerator_Mod(ModContentPack content) : base(content)
        {
            GetSettings<VanillaQuestsExpandedTheGenerator_Settings>();
        }
        public override string SettingsCategory() => "VQE - The Generator";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            VanillaQuestsExpandedTheGenerator_Settings.DoWindowContents(inRect);
        }
    }
}
