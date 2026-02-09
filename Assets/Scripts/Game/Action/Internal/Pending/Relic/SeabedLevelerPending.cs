using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeabedLevelerPending : PendingAction, System.IDisposable, IRelicAction, IInternal
    {
        private SeabedLeveler seabedLeveler;

        public SeabedLevelerPending(SeabedLeveler sl, int maker) : base(maker)
        {
            seabedLeveler = sl;
            Target = (ushort)maker;
            Maker = (ushort)maker;
        }

        public override void CompleteAction()
        {
            var excute = new SeabedLevelerExecute(Maker, Target);
            BoardViewer.Ins.ExecuteAction(excute);

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            seabedLeveler = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}