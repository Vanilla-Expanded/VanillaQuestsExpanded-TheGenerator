using VFECore;
namespace VanillaQuestsExpandedTheGenerator
{
	public class TheGenerator_QuestChainWorker : QuestChainWorker
	{
        public override string GetDescription(QuestChainDef def)
        {
            return base.GetDescription(def).Replace("{InventorFullName}", Genetron_GameComponent.Instance.inventor.NameFullColored); ;
        }
	}
}
