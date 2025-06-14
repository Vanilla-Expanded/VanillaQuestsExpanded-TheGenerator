using Verse;

namespace VanillaQuestsExpandedTheGenerator
{
    public class Building_Genetron_Base : Building
    {
        public ExtraGenetronParameters _cachedDetailsExtension;
        public ExtraGenetronParameters cachedDetailsExtension => _cachedDetailsExtension ??= def.GetModExtension<ExtraGenetronParameters>();

        public override string Label
        {
            get
            {
                if (cachedDetailsExtension != null)
                {
                    if (cachedDetailsExtension.includeInventorFullName)
                    {
                        return "VQE_ThingWithInventorName".Translate(Genetron_GameComponent.Instance.inventor.NameFullColored, def.label);
                    }
                    else if (cachedDetailsExtension.includeInventorFirstName)
                    {
                        return "VQE_ThingWithInventorName".Translate(Genetron_GameComponent.Instance.inventor.NameShortColored, def.label);
                    }
                }
                return base.Label;
            }
        }

        public override string DescriptionDetailed => base.DescriptionDetailed.Replace("{InventorFullName}", Genetron_GameComponent.Instance.inventor.NameFullColored);

        public override string DescriptionFlavor => base.DescriptionFlavor.Replace("{InventorFullName}", Genetron_GameComponent.Instance.inventor.NameFullColored);

    }
}
