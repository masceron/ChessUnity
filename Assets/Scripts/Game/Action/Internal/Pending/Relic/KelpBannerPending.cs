using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class KelpBannerPending : PendingAction, System.IDisposable
    {
        private KelpBanner _kelpBanner;
        public KelpBannerPending(KelpBanner kp, int maker) : base(maker)
        {
            _kelpBanner = kp;
            Maker = maker;
        }
        
        public void Dispose()
        {
            _kelpBanner = null;
            BoardViewer.SelectingFunction = 0;
        }

        protected override void CompleteAction()
        {
            CommitResult(new KelpBannerAction(Maker));
            
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            _kelpBanner.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }
    }
}