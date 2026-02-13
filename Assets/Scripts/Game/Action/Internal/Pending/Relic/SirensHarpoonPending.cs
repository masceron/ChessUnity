using Game.Action.Relics;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class SirensHarpoonPending : PendingAction, System.IDisposable
    {
        private SirensHarpoon _sirensHarpoon;

        public SirensHarpoonPending(SirensHarpoon s, int target) : base(s.CommanderPiece.Pos)
        {
            _sirensHarpoon = s;

            Target = target;
        }

        protected override void CompleteAction()
        {
            _sirensHarpoon.SetCooldown();
            var execute = new SirenHarpoonExecute(Maker, Target);
            CommitResult(execute);

            BoardViewer.Ins.Unmark();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }


        public void Dispose()
        {
            _sirensHarpoon = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}
