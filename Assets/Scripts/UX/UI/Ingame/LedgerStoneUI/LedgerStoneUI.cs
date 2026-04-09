using Game.Common;
using Game.Relics.Commons;

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
            //Làm lại
            //BoardViewer.Ins.ExecuteAction(new LedgerStonePending((LedgerStoneRelic)_relic, true));
        }

        public void SecondOption()
        {
            gameObject.SetActive(false);
            //Làm lại
            //BoardViewer.Ins.ExecuteAction(new LedgerStonePending((LedgerStoneRelic)_relic, false));
        }
    }
}