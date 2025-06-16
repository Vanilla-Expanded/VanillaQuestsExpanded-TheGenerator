using System.Linq;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedTheGenerator
{

	public class Building_Genetron_AncientCryptosleepPod : Building_AncientCryptosleepPod
	{
		public ExtraGenetronParameters _cachedDetailsExtension;
		public ExtraGenetronParameters cachedDetailsExtension => _cachedDetailsExtension ??= def.GetModExtension<ExtraGenetronParameters>();
		public ThingOwner InnerContainer => innerContainer;
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

		public override void EjectContents()
		{
			var inventor = innerContainer.FirstOrDefault() as Pawn;
			base.EjectContents();
			if (inventor != Genetron_GameComponent.Instance.inventor)
			{
				return;
			}
			QuestUtility.SendQuestTargetSignals(Map.Parent.questTags, "InventorReleased", this.Named("SUBJECT"));
			inventor.SetFaction(Faction.OfPlayer);
			Find.LetterStack.ReceiveLetter("LetterLabelRescueeJoins".Translate(inventor.Named("PAWN")),
			"VQE_InventorJoinsDesc".Translate(inventor.Named("PAWN")), LetterDefOf.PositiveEvent, inventor);
			if (Rand.Chance(0.5f))
			{
				inventor.health.AddHediff(InternalDefOf.VQE_TraitorousInventor);
			}
		}

		public override string DescriptionDetailed => base.DescriptionDetailed.Replace("{InventorFullName}", Genetron_GameComponent.Instance.inventor.NameFullColored);

		public override string DescriptionFlavor => base.DescriptionFlavor.Replace("{InventorFullName}", Genetron_GameComponent.Instance.inventor.NameFullColored);

	}
}
