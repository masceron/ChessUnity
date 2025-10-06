
namespace Game.Relics
{
    using Game.Action.Internal.Pending;
    using Game.Action;
    using Game.Piece.PieceLogic;
    using UX.UI.Ingame;
    using Game.Managers;
    using Game.Action.Internal;
    using Game.Effects.Others;
    using Game.Common;

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class EyeOfMimicPending : Action, IPendingAble, System.IDisposable
    {
        public static PieceLogic FirstTarget = null;
        public static PieceLogic SecondTarget = null;
        private EyeOfMimic eyeOfMimic;
        public EyeOfMimicPending(EyeOfMimic e, int maker, bool pos = false) : base(maker, pos)
        {
            eyeOfMimic = e;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
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
            TileManager.Ins.UnmarkAll();

            var ourSide = MatchManager.Ins.GameState.OurSide;

            var source = FirstTarget.Color == ourSide ? FirstTarget : SecondTarget;
            var target = FirstTarget.Color == ourSide ? SecondTarget : FirstTarget;

            ActionManager.ExecuteImmediately(new ApplyEffect(new CopyCapturesMethod(source, target, 0)));

            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            eyeOfMimic.SetCooldown();
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

            eyeOfMimic = null;

            BoardViewer.SelectingFunction = 0;
        }

        protected override void ModifyGameState()
        {
        }
    }

}
