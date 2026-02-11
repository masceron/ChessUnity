using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AdrenalineRadiatorPending : PendingAction, System.IDisposable
    {
        private AdrenalineRadiator _relic;
        public AdrenalineRadiatorPending(AdrenalineRadiator relic, int maker) : base(maker)
        {
            _relic = relic;
            Maker = (ushort)maker;
        }

        public void Dispose()
        {
            _relic = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            _relic.SetCooldown();

            var execute = new AdrenalineRadiatorExcute(Maker);
            CommitResult(execute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            BoardViewer.Ins.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }
    }
}
