using Game.Action.Internal.Pending.Relic;
using Game.Relics.Commons;
using Game.Common;
using LedgerStoneRelic = Game.Relics.LedgerStone;

namespace UX.UI.Ingame.LedgerStone
{
    public class LedgerStoneUI : Singleton<LedgerStoneUI>
    {
        private RelicLogic relic; 
        public void Load(RelicLogic relicLogic)
        {
            relic = relicLogic;
        }
        public void FirstOption()
        {
            gameObject.SetActive(false);
            BoardViewer.Ins.ExecuteAction(new LedgerStonePending((LedgerStoneRelic)relic, true));
            
        }
        public void SecondOption()
        {
            gameObject.SetActive(false);
            BoardViewer.Ins.ExecuteAction(new LedgerStonePending((LedgerStoneRelic)relic, false));
        }
    }
}