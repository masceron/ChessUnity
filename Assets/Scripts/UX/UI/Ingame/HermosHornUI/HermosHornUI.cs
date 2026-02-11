using Game.Action.Relics;
using Game.Common;
using Game.Relics.Commons;

namespace UX.UI.Ingame.HermosHornUI
{
    public class HermosHornUI : Singleton<HermosHornUI>
    {
        private RelicLogic _relic; 
        public void Load(RelicLogic relicLogic)
        {
            _relic = relicLogic;
        }
        public void FirstOption()
        {
            BoardViewer.Ins.ExecuteAction(new HermosHornExcute(_relic.Color, true));
            _relic.SetCooldown();
        }
        public void SecondOption()
        {
            BoardViewer.Ins.ExecuteAction(new HermosHornExcute(_relic.Color, false));
            _relic.SetCooldown();
        }
    }
}