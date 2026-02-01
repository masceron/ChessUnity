using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class KelpBannerPending : PendingAction, System.IDisposable
    {
        private KelpBanner kelpBanner;
        public KelpBannerPending(KelpBanner kp, int maker, bool pos = false) : base(maker)
        {
            kelpBanner = kp;
            Maker = (ushort)maker;
        }

        

        public void Dispose()
        {
            kelpBanner = null;
            BoardViewer.SelectingFunction = 0;
        }

        public override void CompleteAction()
        {
            BoardViewer.Ins.ExecuteAction(new KelpBannerAction(Maker));
            
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            kelpBanner.SetCooldown();
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }
    }
}