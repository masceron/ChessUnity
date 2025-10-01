
namespace Game.Relics
{
    using Game.Action.Internal.Pending;
    using Game.Action;
    using Game.Piece.PieceLogic;
    using UX.UI.Ingame;
    using Game.Managers;
    using UnityEngine;
    using Game.Action.Internal;
    using Game.Effects.Others;

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class EyeOfMimicPending : Action, IPendingAble
    {
        public static PieceLogic FristTarget = null;
        public static PieceLogic SecondTarget = null;
        private EyeOfMimic eyeOfMimic;
        public EyeOfMimicPending(EyeOfMimic e, int maker, bool pos = false) : base(maker, pos)
        {
            eyeOfMimic = e;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            if (FristTarget == null)
            {
                FristTarget = BoardViewer.Hovering;
                TileManager.Ins.MarkIfDifferntColor(FristTarget.Color);
                TileManager.Ins.Select(FristTarget.Pos);
                return;
            }
            else
            {
                if (FristTarget.Color == BoardViewer.Hovering.Color) return;

                SecondTarget = BoardViewer.Hovering;
                eyeOfMimic.SetCooldown();

                TileManager.Ins.UnmarkAll();

                ActionManager.ExecuteImmediately(new ApplyEffect (new SwapMoveMethod(FristTarget, SecondTarget, 0, true)));
                ActionManager.ExecuteImmediately(new ApplyEffect(new SwapMoveMethod(SecondTarget, FristTarget, 0, false)));

                BoardViewer.Selecting = -1;
                BoardViewer.SelectingFunction = 0;
                MatchManager.Ins.InputProcessor.UpdateRelic();
            }
        }

        protected override void ModifyGameState()
        {
        }
    }

}
