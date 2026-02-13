using System;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AdrenalineRadiatorPending : PendingAction, IDisposable
    {
        private AdrenalineRadiator _relic;

        public AdrenalineRadiatorPending(AdrenalineRadiator relic, int maker) : base(maker)
        {
            _relic = relic;
            Maker = maker;
        }

        public void Dispose()
        {
            _relic = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            _relic.SetCooldown();

            var execute = new AdrenalineRadiatorExecute(Maker);
            CommitResult(execute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            BoardViewer.Ins.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }
    }
}