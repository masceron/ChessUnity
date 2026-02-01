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
    public class ReliquaryPending : PendingAction, System.IDisposable, IRelicAction, IInternal
    {
        private Reliquary reliquary;
        
        public ReliquaryPending(Reliquary cp, int maker, bool pos = false) : base(maker)
        {
            reliquary = cp;
            Target = (ushort)maker;
            Maker = (ushort)maker;
        }

        public override void CompleteAction()
        {
            if (reliquary != null)
            {
                reliquary.SetCooldown();
            }
            
            var excute = new ReliquaryExcute();
            BoardViewer.Ins.ExecuteAction(excute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            reliquary = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}