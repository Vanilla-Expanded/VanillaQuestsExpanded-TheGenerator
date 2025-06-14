using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;

namespace VanillaQuestsExpandedTheGenerator
{
	public class QuestPart_Root_AbandonedARC : QuestPartActivable
	{
		public Site site;
		public string signalStudiedAll;
		public string signalObjectDestroyed;
		public string signalMoreThanHalfDestroyed;
		public int initialStudiableCount;
		public int countDestroyed;
		
		public QuestPart_Root_AbandonedARC()
		{
		}

		public QuestPart_Root_AbandonedARC(Site site, string mapGenerated, string signalStudiedAll, 
			string signalMoreThanHalfDestroyed, string siteObjectDestroyed)
		{
			inSignalEnable = mapGenerated;
			this.site = site;
			this.signalStudiedAll = signalStudiedAll;
			this.signalMoreThanHalfDestroyed = signalMoreThanHalfDestroyed;
			this.signalObjectDestroyed = siteObjectDestroyed;
		}

		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			
			if (signal.tag == signalObjectDestroyed)
			{
				countDestroyed++;
			}
		}

		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			initialStudiableCount = site.Map.listerThings.AllThings.OfType<Building_Genetron_Studiable>().Count();
		}
		
		public override void QuestPartTick()
		{
			if (countDestroyed > initialStudiableCount / 2f)
			{
				Find.SignalManager.SendSignal(new Signal(signalMoreThanHalfDestroyed, site.Named("SUBJECT")));
				QuestUtility.SendQuestTargetSignals(site.questTags, "MoreThanHalfObjectsDestroyed", site.Named("SUBJECT"));
				foreach (var building in site.Map.listerThings.AllThings.OfType<Building_Genetron_Studiable>())
				{
					building.GetComp<CompBouncingArrow>().doBouncingArrow = false;
				}
			}
			else if (site.Map.listerThings.AllThings.OfType<Building_Genetron_Studiable>()
			.Count(x => x.studied is false) == 0)
			{
				Find.SignalManager.SendSignal(new Signal(signalStudiedAll, site.Named("SUBJECT")));
				QuestUtility.SendQuestTargetSignals(site.questTags, "StudiedAll", site.Named("SUBJECT"));
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look(ref site, "site");
			Scribe_Values.Look(ref signalStudiedAll, "signalStudiedAll");
			Scribe_Values.Look(ref signalMoreThanHalfDestroyed, "signalMoreThanHalfDestroyed");
			Scribe_Values.Look(ref signalObjectDestroyed, "signalObjectDestroyed");
			Scribe_Values.Look(ref initialStudiableCount, "studiableCount");
			Scribe_Values.Look(ref countDestroyed, "countDestroyed");
		}
	}
}
