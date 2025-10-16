
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Effects.Others;
using Game.Managers;
using Game.Piece.PieceLogic;
using UX.UI.Ingame;

namespace Game.Relics.SirensHarpoon
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class SirensHarpoonPending : Action.Action, IPendingAble, System.IDisposable
    {
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        private SirensHarpoon _sirensHarpoon;
        public SirensHarpoonPending(SirensHarpoon s, int maker, bool pos = false) : base(maker, pos)
        {
            _sirensHarpoon = s;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            var hovering = BoardUtils.PieceOn(BoardViewer.HoveringPos);

            if (FirstTarget == null)
            {
                FirstTarget = hovering;
                TileManager.Ins.Select(FirstTarget.Pos);
                TileManager.Ins.MarkAsMoveable(FirstTarget.Pos);
            }

            //TileManager.Ins.UnmarkAll();

         

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            _sirensHarpoon.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic();

            ResetTargets();
        }

        private static void ResetTargets()
        {
            FirstTarget = null;
            SecondTarget = null;
        }

        public void Dispose()
        {
            ResetTargets();

            _sirensHarpoon = null;

            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
        }
    }

}
