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
        public static PieceLogic FirstTarget;
        public static PieceLogic SecondTarget;
        private EyeOfMimic eyeOfMimic;
        public EyeOfMimicPending(EyeOfMimic e, int maker, bool pos = false) : base(maker)
        {
            eyeOfMimic = e;
            Maker = (ushort)maker;
        }

        public override void CompleteAction()
        {
            var hovering = BoardUtils.PieceOn(BoardViewer.HoveringPos);

            if (FirstTarget == null || FirstTarget.Color == hovering.Color)
            {
                FirstTarget = hovering;
                TileManager.Ins.MarkIfDifferntColor(FirstTarget.Color);
                TileManager.Ins.Select(FirstTarget.Pos);
                return;
            }

            SecondTarget = hovering;

            var ourSide = eyeOfMimic.Color;
            var source = FirstTarget.Color == ourSide ? FirstTarget : SecondTarget;
            var target = FirstTarget.Color == ourSide ? SecondTarget : FirstTarget;

            EyeOfMimicExcute excute = new EyeOfMimicExcute(source.Pos, target.Pos);
            eyeOfMimic.SetCooldown();
            BoardViewer.Ins.ExecuteAction(excute);
            TileManager.Ins.UnmarkAll();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }

        private static void ResetTargets()
        {
            FirstTarget = null;
            SecondTarget = null;
        }

        public void Dispose()
        {
            ResetTargets();
            eyeOfMimic = null;
            BoardViewer.SelectingFunction = 0;
        }
    }

}
