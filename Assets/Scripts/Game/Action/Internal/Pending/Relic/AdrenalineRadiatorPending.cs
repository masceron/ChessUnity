using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AdrenalineRadiatorPending : PendingAction, System.IDisposable
    {
        private AdrenalineRadiator relic;
        public AdrenalineRadiatorPending(AdrenalineRadiator relic, int maker) : base(maker)
        {
            this.relic = relic;
            Maker = (ushort)maker;
        }

        public void Dispose()
        {
            relic = null;
            BoardViewer.SelectingFunction = 0;
        }

        public override void CompleteAction()
        {
            relic.SetCooldown();

            var excute = new AdrenalineRadiatorExcute(Maker);
            BoardViewer.Ins.ExecuteAction(excute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            BoardViewer.Ins.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }
    }
}
