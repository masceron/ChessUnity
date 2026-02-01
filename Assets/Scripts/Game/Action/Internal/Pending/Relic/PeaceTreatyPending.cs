using Game.Common;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;
using Game.Action.Internal.Pending;
using Game.Action.Internal;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PeaceTreatyPending : PendingAction, System.IDisposable, IRelicAction, IInternal
    {
        private PeaceTreaty peaceTreaty;
        
        public PeaceTreatyPending(PeaceTreaty cp, int maker, bool pos = false) : base(maker)
        {
            peaceTreaty = cp;
            Target = (ushort)maker;
            Maker = (ushort)maker;
        }

        public override void CompleteAction()
        {
            
            var excute = new PeaceTreatyExcute(peaceTreaty.Color);
            BoardViewer.Ins.ExecuteAction(excute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.Unmark();
            peaceTreaty.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            peaceTreaty = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}   