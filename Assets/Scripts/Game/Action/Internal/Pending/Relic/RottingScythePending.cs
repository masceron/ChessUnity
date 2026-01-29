using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RottingScythePending : PendingAction, System.IDisposable
    {
        private RottingScythe rottingScythe;

        public RottingScythePending(RottingScythe rottingScythe, int maker, bool pos) : base(maker)
        {
            this.rottingScythe = rottingScythe;
            Maker = (ushort)maker;
        }

        public override void CompleteAction()
        {
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            rottingScythe.SetCooldown();
            BoardViewer.Ins.ExecuteAction(new RottingScytheAction(Maker));
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }
        
        public void Dispose()
        {
            rottingScythe = null;
            BoardViewer.SelectingFunction = 0;
        }

    }
}