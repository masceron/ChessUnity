using System;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RottingScythePending : PendingAction, IDisposable
    {
        private RottingScythe _rottingScythe;

        public RottingScythePending(RottingScythe rottingScythe, int maker) : base(maker)
        {
            _rottingScythe = rottingScythe;
        }

        public void Dispose()
        {
            _rottingScythe = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            _rottingScythe.SetCooldown();
            CommitResult(new RottingScytheAction(GetFrom()));
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }
    }
}