using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;
using Game.Effects.Others;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeabedLevelerPending : PendingAction, System.IDisposable, IInternal
    {
        private SeabedLeveler seabedLeveler;
        private Charge charges;

        public SeabedLevelerPending(SeabedLeveler sl, Charge _charge, int maker) : base(maker)
        {
            seabedLeveler = sl;
            Target = (ushort)maker;
            Maker = (ushort)maker;
            charges = _charge;
        }

        public override void CompleteAction()
        {
            var excute = new SeabedLevelerExecute(Maker, Target);
            BoardViewer.Ins.ExecuteAction(excute);
            charges.Strength -= 3;

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