using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class SirensHarpoonPending : PendingAction, System.IDisposable
    {
        private SirensHarpoon _sirensHarpoon;

        public SirensHarpoonPending(SirensHarpoon s, int target, bool pos = false) : base(s.CommanderPiece.Pos)
        {
            _sirensHarpoon = s;

            Target = (ushort)target;
        }

        public override void CompleteAction()
        {
            _sirensHarpoon.SetCooldown();
            var excute = new SirenHarpoonExcute(Maker, Target);
            BoardViewer.Ins.ExecuteAction(excute);

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
