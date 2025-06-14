using VEF.Storyteller;
namespace VanillaQuestsExpandedTheGenerator
{
	public class TheGenerator_QuestChainWorker : QuestChainWorker
	{
        public override string GetDescription()
        {
            return base.GetDescription().Replace("{InventorFullName}", Genetron_GameComponent.Instance.inventor.NameFullColored); ;
        }
	}
}
