using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TimelessHourglassPending : PendingAction, System.IDisposable
    {
        private TimelessHourglass _timelessHourglass;

        public TimelessHourglassPending(TimelessHourglass t, int target) : base(t.CommanderPiece.Pos)
        {
            _timelessHourglass = t;
            Target = target;
        }

        protected override void CompleteAction()
        {
            _timelessHourglass.SetCooldown();

            var execute = new TimelessHourglassExecute(Maker, _timelessHourglass.Color, Target);
            CommitResult(execute);

            BoardViewer.Ins.Unmark();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }

        public void Dispose()
        {
            _timelessHourglass = null;
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
        }
    }
}