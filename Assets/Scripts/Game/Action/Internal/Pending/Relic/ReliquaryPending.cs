using System;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ReliquaryPending : PendingAction, IDisposable, IRelicAction
    {
        private Reliquary _reliquary;

        public ReliquaryPending(Reliquary cp, int maker) : base(maker)
        {
            _reliquary = cp;
            Target = maker;
            Maker = maker;
        }

        public void Dispose()
        {
            _reliquary = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            _reliquary?.SetCooldown();

            var execute = new ReliquaryExecute();
            CommitResult(execute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }
    }
}