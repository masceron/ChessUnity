using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Relics.Commons;
using LedgerStoneRelic = Game.Relics.LedgerStone;

namespace UX.UI.Ingame.LedgerStoneUI
{
    public class LedgerStoneUI : Singleton<LedgerStoneUI>
    {
        private RelicLogic _relic; 
        public void Load(RelicLogic relicLogic)
        {
            _relic = relicLogic;
        }
        public void FirstOption()
        {
            gameObject.SetActive(false);
            BoardViewer.Ins.ExecuteAction(new LedgerStonePending((LedgerStoneRelic)_relic, true));
            
        }
        public void SecondOption()
        {
            gameObject.SetActive(false);
            BoardViewer.Ins.ExecuteAction(new LedgerStonePending((LedgerStoneRelic)_relic, false));
        }
    }
}