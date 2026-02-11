using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UX.UI.Ingame;
using Game.Action.Relics;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class EyeOfMimicPending : PendingAction, System.IDisposable
    {
        private static PieceLogic _firstTarget;
        private static PieceLogic _secondTarget;
        private EyeOfMimic _eyeOfMimic;
        public EyeOfMimicPending(EyeOfMimic e, int maker) : base(maker)
        {
            _eyeOfMimic = e;
            Maker = (ushort)maker;
        }

        protected override void CompleteAction()
        {
            var hovering = BoardUtils.PieceOn(BoardViewer.HoveringPos);

            if (_firstTarget == null || _firstTarget.Color == hovering.Color)
            {
                _firstTarget = hovering;
                TileManager.Ins.MarkIfDifferntColor(_firstTarget.Color);
                TileManager.Ins.Select(_firstTarget.Pos);
                return;
            }

            _secondTarget = hovering;

            var ourSide = _eyeOfMimic.Color;
            var source = _firstTarget.Color == ourSide ? _firstTarget : _secondTarget;
            var target = _firstTarget.Color == ourSide ? _secondTarget : _firstTarget;

            var execute = new EyeOfMimicExcute(source.Pos, target.Pos);
            _eyeOfMimic.SetCooldown();
            CommitResult(execute);
            TileManager.Ins.UnmarkAll();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }

        private static void ResetTargets()
        {
            _firstTarget = null;
            _secondTarget = null;
        }

        public void Dispose()
        {
            ResetTargets();
            _eyeOfMimic = null;
            BoardViewer.SelectingFunction = 0;
        }
    }

}
