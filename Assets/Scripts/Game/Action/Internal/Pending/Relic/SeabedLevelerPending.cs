using Game.Managers;
using UX.UI.Ingame;
using Game.Action.Relics;
using Game.Effects.Others;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeabedLevelerPending : PendingAction, System.IDisposable
    {
        private readonly Charge _charges;

        public SeabedLevelerPending(Charge charge, int maker) : base(maker)
        {
            Target = (ushort)maker;
            Maker = (ushort)maker;
            _charges = charge;
        }

        protected override void CompleteAction()
        {
            var execute = new SeabedLevelerExecute(Maker, Target);
            CommitResult(execute);
            _charges.Strength -= 3;

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            MatchManager.Ins.InputProcessor.Unmark();
            MatchManager.Ins.InputProcessor.UpdateRelic();
            Dispose();
        }

        public void Dispose()
        {
            BoardViewer.SelectingFunction = 0;
        }
    }
}