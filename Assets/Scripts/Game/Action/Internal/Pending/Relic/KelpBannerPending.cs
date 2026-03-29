using System;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class KelpBannerPending : PendingAction, IDisposable
    {
        private KelpBanner _kelpBanner;

        public KelpBannerPending(KelpBanner kp) : base(null)
        {
            _kelpBanner = kp;
        }

        public void Dispose()
        {
            _kelpBanner = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            CommitResult(new KelpBannerAction(GetFrom()));

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            _kelpBanner.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }
    }
}