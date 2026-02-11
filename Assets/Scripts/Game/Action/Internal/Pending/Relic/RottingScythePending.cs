using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RottingScythePending : PendingAction, System.IDisposable
    {
        private RottingScythe _rottingScythe;

        public RottingScythePending(RottingScythe rottingScythe, int maker) : base(maker)
        {
            _rottingScythe = rottingScythe;
            Maker = (ushort)maker;
        }

        protected override void CompleteAction()
        {
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            _rottingScythe.SetCooldown();
            CommitResult(new RottingScytheAction(Maker));
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }
        
        public void Dispose()
        {
            _rottingScythe = null;
            BoardViewer.SelectingFunction = 0;
        }

    }
}